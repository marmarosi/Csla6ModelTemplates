using Csla.Configuration;
using Csla6ModelTemplates.Configuration;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1)]
namespace Csla6ModelTemplates.EndpointTests
{
    /// <summary>
    /// Provides methods to initialize integration tests.
    /// </summary>
    internal class TestSetup
    {
        private static readonly TestSetup instance = new();
        private readonly IServiceCollection services = new ServiceCollection();
        private readonly ServiceProvider provider;

        public ICslaService Csla { get; private set; }

        /// <summary>
        /// Initializes an integration test.
        /// </summary>
        private TestSetup()
        {
            // Set configuration.
            IConfiguration configuration = ConfigurationCreator.Create();
            services.AddSingleton(configuration);
            //var webRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var basePath = Path.Join(webRootPath, "../../..");
            //var sharedSettings = Path.Join(basePath, "../Shared/SharedSettings.json");

            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile(sharedSettings, true, true)
            //    .AddEnvironmentVariables();
            //IConfiguration configuration = builder.Build();

            // Configure data access layer.
            services.AddDataAccessLayers(configuration);
            //IDeadLockDetector detector = new DeadLockDetector();
            //services.AddSingleton(detector);
            //services.AddMySqlDal(detector, configuration);
            ////services.AddPostgreSqlDal(detector, configuration);
            ////services.AddSqlServerDal(detector, configuration);
            //services.AddSingleton(typeof(ITransactionOptions), new TransactionOptions(true));

            // Configure CSLA.
            services.AddCsla();
            services.AddScoped<ICslaService, CslaService>();

            // Initialize properties.
            provider = services.BuildServiceProvider();
            Csla = provider.GetRequiredService<ICslaService>();
        }

        /// <summary>
        /// Yields the singleton setup instance.
        /// </summary>
        /// <returns>The singleton setup instance.</returns>
        public static TestSetup GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// Gets a dummy logger for a controller.
        /// </summary>
        /// <typeparam name="T">Th etype of the controller.</typeparam>
        /// <returns>The dummy logger.</returns>
        public ILogger<T> GetLogger<T>() where T : class
        {
            // Create logger.
            return new NullLogger<T>();
        }
    }
}
