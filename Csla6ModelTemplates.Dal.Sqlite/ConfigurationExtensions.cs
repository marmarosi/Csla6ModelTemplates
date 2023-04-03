using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Dal.Sqlite;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csla6ModelTemplates.Configuration
{
    /// <summary>
    /// Provide methods to configure SQLite databases.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add the services to Entity Framewprk to use SQLite.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Teh application configuration.</param>
        public static void AddSqliteDal(
            this IServiceCollection services,
            IDeadLockDetector detector = null,
            IConfiguration configuration = null
            )
        {
            // Configure database.
            if (configuration == null)
                services.AddDbContext<SqliteContext>(options => options
                    .UseSqlite($"name=ConnectionStrings:{DAL.SQLite}")
                    );
            else
                services.AddDbContext<SqliteContext>(options =>
                    options.UseSqlite(configuration.GetConnectionString(DAL.SQLite))
                    );

            // Configure data access layer.
            foreach (var dalType in SqliteDalIndex.Items)
                services.AddTransient(dalType.Key, dalType.Value);

            // Configure dead lock checking.
            detector.RegisterCheckMethod(
                DAL.SQLite,
                typeof(ConfigurationExtensions).GetMethod("IsSqliteDeadlock")
                );
        }

        /// <summary>
        /// CHecks whether the reason of the exception is a deadlock.
        /// </summary>
        /// <param name="ex">The original exception thrown.</param>
        /// <returns>True when the reason is a deadlock; otherwise false;</returns>
        public static bool IsSqliteDeadlock(
            Exception ex
            )
        {
            return ex is SqliteException && (ex as SqliteException).ErrorCode == 6; // SQLITE_LOCKED
        }

        /// <summary>
        /// Runs SQLite seeders.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="isDevelopment">Indicates whether the hosting environment is development.</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void RunSqliteSeeders(
            this IApplicationBuilder app,
            bool isDevelopment,
            string contentRootPath
            )
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SqliteContext>();

                SqliteSeeder.Run(context, isDevelopment, contentRootPath);
            }
        }
    }
}
