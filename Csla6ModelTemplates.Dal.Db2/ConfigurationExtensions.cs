using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Dal.Db2;
using IBM.Data.Db2;
using IBM.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csla6ModelTemplates.Configuration
{
    /// <summary>
    /// Provide methods to configure DB2 databases.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add the services to Entity Framewprk to use DB2.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Teh application configuration.</param>
        public static void AddDb2Dal(
            this IServiceCollection services,
            IDeadLockDetector detector = null,
            IConfiguration configuration = null
            )
        {
            // Configure database.
            if (configuration == null)
                services.AddDbContext<Db2Context>(options => options
                    .UseDb2($"name=ConnectionStrings:{DAL.DB2}", null)
                    );
            else
                services.AddDbContext<Db2Context>(options =>
                    options.UseDb2(configuration.GetConnectionString(DAL.DB2), null)
                    );

            // Configure data access layer.
            foreach (var dalType in Db2DalIndex.Items)
                services.AddTransient(dalType.Key, dalType.Value);

            // Configure dead lock checking.
            detector.RegisterCheckMethod(
                DAL.DB2,
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
            if (ex is DB2Exception)
            {
                switch ((ex as DB2Exception).ErrorCode)
                {
                    /* The current unit of work was the victim in a deadlock, or experienced a timeout,
                     * and had to be rolled back. */
                    case -911:
                    /* The application was the victim in a deadlock or experienced a timeout.
                     * The reason code indicates whether a deadlock or timeout occurred. */
                    case -913:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Runs DB2 seeders.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="isDevelopment">Indicates whether the hosting environment is development.</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void RunDb2Seeders(
            this IApplicationBuilder app,
            bool isDevelopment,
            string contentRootPath
            )
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Db2Context>();

                Db2Seeder.Run(context, isDevelopment, contentRootPath);
            }
        }
    }
}
