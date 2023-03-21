using Csla;
using Csla.Configuration;
using Csla6ModelTemplates.Configuration;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1)]
namespace Csla6ModelTemplates.WebApiTests
{
    /// <summary>
    /// Provides methods to initialize integration tests.
    /// </summary>
    internal class TestSetup
    {
        private static readonly TestSetup instance = new();
        private readonly IServiceCollection services = new ServiceCollection();
        private readonly ApplicationContext appContext;
        private readonly ServiceProvider provider;

        public IDataPortalFactory PortalFactory { get; private set; }
        public IChildDataPortalFactory ChildPortalFactory { get; private set; }

        /// <summary>
        /// Initializes an integration test.
        /// </summary>
        private TestSetup()
        {
            // Set configuration.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("SharedSettings.json", true, true)
                .AddJsonFile("appsettings.json", true, true) // use appsettings.json in current folder
                .AddEnvironmentVariables();
            IConfiguration configuration = builder.Build();
            DeadLockDetector detector = new DeadLockDetector();

            // Configure data access layer.
            services.AddSqlServerDal(detector, configuration);
            services.AddSingleton(typeof(ITransactionOptions), new TransactionOptions(true));

            // Configure CSLA.
            services.AddCsla();
            provider = services.BuildServiceProvider();
            appContext = provider.GetRequiredService<ApplicationContext>();
            PortalFactory = provider.GetRequiredService<IDataPortalFactory>();
            ChildPortalFactory = provider.GetRequiredService<IChildDataPortalFactory>();
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

        /// <summary>
        /// Gets the CSLA data portal for the specified model, collection or command.
        /// </summary>
        /// <typeparam name="T">The type of the model, collection or command.</typeparam>
        /// <returns>The required CSLA data portal.</returns>
        public DataPortal<T> GetPortal<T>() where T : class
        {
            // Create logger.
            return appContext.CreateInstanceDI<DataPortal<T>>();
        }
    }
}
