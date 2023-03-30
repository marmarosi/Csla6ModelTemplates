using Csla6ModelTemplates.Configuration;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Csla6ModelTemplates.WebApiTests
{
    /// <summary>
    /// Provide methods to configure data access layers.
    /// </summary>
    internal static class DataAccessExtensions
    {
        /// <summary>
        /// Add the services to Entity Framewprk to use data access layers.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The application configuration.</param>
        public static void AddDataAccessLayers(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            var dalNames = configuration.GetSection("ActiveDals").Get<List<string>>();

            IDeadLockDetector detector = new DeadLockDetector();
            services.AddSingleton(detector);

            foreach (var dalName in dalNames)
            {
                switch (dalName)
                {
                    case DAL.MySQL:
                        services.AddMySqlDal(detector);
                        break;
                    case DAL.Oracle:
                        services.AddOracleDal(detector);
                        break;
                    case DAL.PostgreSQL:
                        services.AddPostgreSqlDal(detector);
                        break;
                    case DAL.SQLite:
                        break;
                    case DAL.SQLServer:
                        services.AddSqlServerDal(detector);
                        break;
                }
            }
            services.AddSingleton(typeof(ITransactionOptions), new TransactionOptions(false));
        }
    }
}
