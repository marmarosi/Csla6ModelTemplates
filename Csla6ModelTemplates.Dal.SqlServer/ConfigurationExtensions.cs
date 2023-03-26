using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Dal.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csla6ModelTemplates.Configuration
{
    /// <summary>
    /// Configuration extension methods
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add the services to Entity Framewprk to use SQL Server.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Teh application configuration.</param>
        public static void AddSqlServerDal(
            this IServiceCollection services,
            IDeadLockDetector detector = null,
            IConfiguration configuration = null
            )
        {
            // Configure database.
            if (configuration == null)
                services.AddDbContext<SqlServerContext>(options => options
                    .UseSqlServer($"name=ConnectionStrings:{DAL.SQLServer}")
                    );
            else
                services.AddDbContext<SqlServerContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString(DAL.SQLServer))
                );

            // Configure data access layer.
            foreach (var dalType in SqlServerDalIndex.Items)
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
            return ex is SqlException && (ex as SqlException).Number == 1205;
        }

        /// <summary>
        /// Runs seeders of persistent storages.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="isDevelopment">Indicates whether the hosting environment is development..</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void RunSqlServerSeeders(
            this IApplicationBuilder app,
            bool isDevelopment,
            string contentRootPath
            )
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<SqlServerContext>();

                SqlServerSeeder.Run(context, isDevelopment, contentRootPath);
            }
        }
    }
}
