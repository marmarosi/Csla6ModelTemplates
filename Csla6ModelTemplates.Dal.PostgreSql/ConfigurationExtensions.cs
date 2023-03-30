using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Dal.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Csla6ModelTemplates.Configuration
{
    /// <summary>
    /// Provide methods to configure PostgreSQL databases.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add the services to Entity Framewprk to use PostgreSQL.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Teh application configuration.</param>
        public static void AddPostgreSqlDal(
            this IServiceCollection services,
            IDeadLockDetector detector = null,
            IConfiguration configuration = null
            )
        {
            // Configure database.
            if (configuration == null)
                services.AddDbContext<PostgreSqlContext>(options => options
                    .UseNpgsql($"name=ConnectionStrings:{DAL.PostgreSQL}")
                    );
            else
                services.AddDbContext<PostgreSqlContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString(DAL.PostgreSQL))
                    );

            // Configure data access layer.
            foreach (var dalType in PostgreSqlDalIndex.Items)
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
            return ex is PostgresException && (ex as PostgresException).SqlState == "40P01";
        }

        /// <summary>
        /// Runs PostgreSQL seeders.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="isDevelopment">Indicates whether the hosting environment is development..</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void RunPostgreSqlSeeders(
            this IApplicationBuilder app,
            bool isDevelopment,
            string contentRootPath
            )
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PostgreSqlContext>();

                PostgreSqlSeeder.Run(context, isDevelopment, contentRootPath);
            }
        }
    }
}
