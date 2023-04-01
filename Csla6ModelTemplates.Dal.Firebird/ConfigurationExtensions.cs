using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Dal.Firebird;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csla6ModelTemplates.Configuration
{
    /// <summary>
    /// Provide methods to configure Firebird databases.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add the services to Entity Framewprk to use Firebird.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">Teh application configuration.</param>
        public static void AddFirebirdDal(
            this IServiceCollection services,
            IDeadLockDetector detector = null,
            IConfiguration configuration = null
            )
        {
            // Configure database.
            if (configuration == null)
                services.AddDbContext<FirebirdContext>(options => options
                    .UseFirebird($"name=ConnectionStrings:{DAL.Firebird}")
                    );
            else
                services.AddDbContext<FirebirdContext>(options =>
                    options.UseFirebird(configuration.GetConnectionString(DAL.Firebird))
                    );

            // Configure data access layer.
            foreach (var dalType in FirebirdDalIndex.Items)
                services.AddTransient(dalType.Key, dalType.Value);

            // Configure dead lock checking.
            detector.RegisterCheckMethod(
                DAL.Firebird,
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
            return ex is FbException && (ex as FbException).ErrorCode == 40001;
            //if (ex is FbException)
            //{
            //    switch ((ex as FbException).ErrorCode)
            //    {
            //        /* SQLSTATE = SQLCLASS 40 (Transaction Rollback) */
            //        case 40000: /* Ongoing transaction has been rolled back */
            //        case 40001: /* Serialization failure  */ <= THIS IS DEADLOCK
            //        case 40002: /* Transaction integrity constraint violation */
            //        case 40003: /* Statement completion unknown */
            //            return true;
            //        default:
            //            break;
            //    }
            //}
            //return false;
        }

        /// <summary>
        /// Runs Firebird seeders.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="isDevelopment">Indicates whether the hosting environment is development..</param>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void RunFirebirdSeeders(
            this IApplicationBuilder app,
            bool isDevelopment,
            string contentRootPath
            )
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FirebirdContext>();

                FirebirdSeeder.Run(context, isDevelopment, contentRootPath);
            }
        }
    }
}
