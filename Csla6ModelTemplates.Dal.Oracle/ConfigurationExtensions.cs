using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Dal.Oracle;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace Csla6ModelTemplates.Configuration
{
    /// <summary>
    /// Provide methods to configure Oracle databases.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add the services to Entity Framewprk to use Oracle.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Teh application configuration.</param>
        public static void AddOracleDal(
            this IServiceCollection services,
            IDeadLockDetector detector = null,
            IConfiguration configuration = null
            )
        {
            // Configure database.
            if (configuration == null)
                services.AddDbContext<OracleContext>(options => options
                    .UseOracle($"name=ConnectionStrings:{DAL.Oracle}")
                    );
            else
                services.AddDbContext<OracleContext>(options =>
                    options.UseOracle(configuration.GetConnectionString(DAL.Oracle))
                    );

            // Configure data access layer.
            foreach (var dalType in OracleDalIndex.Items)
                services.AddTransient(dalType.Key, dalType.Value);

            // Configure dead lock checking.
            detector.RegisterCheckMethod(
                DAL.Oracle,
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
            if (ex is OracleException)
            {
                switch ((ex as OracleException).Number)
                {
                    //case -2: /* Client Timeout */
                    //case 701: /* Out of Memory */
                    //case 1204: /* Lock Issue */
                    //case 1205: /* Deadlock Victim */
                    //case 1222: /* Lock Request Timeout */
                    //case 8645: /* Timeout waiting for memory resource */
                    //case 8651: /* Low memory condition */

                    case 104:  /* Deadlock detected; all public servers blocked waiting for resources */
                    case 1013: /* User requested cancel of current operation */
                    case 2087: /* Object locked by another process in same transaction */
                    case 60:   /* Deadlock detected while waiting for resource */
                        return true;
                    default:
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Runs Oracle seeders.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="isDevelopment">Indicates whether the hosting environment is development..</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void RunOracleSeeders(
            this IApplicationBuilder app,
            bool isDevelopment,
            string contentRootPath
            )
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<OracleContext>();

                OracleSeeder.Run(context, isDevelopment, contentRootPath);
            }
        }
    }
}
