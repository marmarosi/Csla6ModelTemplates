using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Dal.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace Csla6ModelTemplates.Configuration
{
    /// <summary>
    /// Configuration extension methods
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add the services to Entity Framewprk to use MySQL.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Teh application configuration.</param>
        public static void AddMySqlDal(
            this IServiceCollection services,
            IDeadLockDetector detector = null,
            IConfiguration configuration = null
            )
        {
            // Configure database.
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            if (configuration == null)
                services.AddDbContext<MySqlContext>(options => options
                    .UseMySql($"name=ConnectionStrings:{DAL.MySQL}", serverVersion)
                    );
            else
                services.AddDbContext<MySqlContext>(options =>
                    options.UseMySql(configuration.GetConnectionString(DAL.MySQL), serverVersion)
                    );

            // Configure data access layer.
            foreach (var dalType in MySqlDalIndex.Items)
                services.AddTransient(dalType.Key, dalType.Value);

            // Configure dead lock checking.
            detector.RegisterCheckMethod(
                DAL.SQLServer,
                typeof(ConfigurationExtensions).GetMethod("IsDeadlock")
                );
        }

        /// <summary>
        /// CHecks whether the reason of the exception is a deadlock.
        /// </summary>
        /// <param name="ex">The original exception thrown.</param>
        /// <returns>True when the reason is a deadlock; otherwise false;</returns>
        public static bool IsDeadlock(
            Exception ex
            )
        {
            return ex is MySqlException && (ex as MySqlException).Number == 1213;
        }

        /// <summary>
        /// Runs seeders of persistent storages.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="isDevelopment">Indicates whether the hosting environment is development..</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void RunMySqlSeeders(
            this IApplicationBuilder app,
            bool isDevelopment,
            string contentRootPath
            )
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<MySqlContext>();

                MySqlSeeder.Run(context, isDevelopment, contentRootPath);
            }
        }
    }
}
