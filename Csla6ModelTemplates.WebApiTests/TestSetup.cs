using Csla;
using Csla.Configuration;
using Csla6ModelTemplates.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1)]
namespace Csla6ModelTemplates.WebApiTests
{
    internal class TestSetup
    {
        private static readonly TestSetup instance = new TestSetup();
        private readonly IServiceCollection services = new ServiceCollection();
        private readonly ApplicationContext appContext;
        private readonly ServiceProvider provider;

        private TestSetup()
        {
            // Set configuration.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("SharedSettings.json", true, true)
                .AddJsonFile("appsettings.json", true, true) // use appsettings.json in current folder
                .AddEnvironmentVariables();
            IConfiguration configuration = builder.Build();

            // Configure data access layer.
            services.AddSqlServerDal(configuration);

            // Configure CSLA.
            services.AddCsla();
            provider = services.BuildServiceProvider();
            appContext = provider.GetRequiredService<ApplicationContext>();
        }

        public static TestSetup GetInstance()
        {
            return instance;
        }

        public ILogger<T> GetLogger<T>() where T : class
        {
            // Create logger.
            return new NullLogger<T>();
        }

        public DataPortal<T> GetPortal<T>() where T : class
        {
            // Create logger.
            return appContext.CreateInstanceDI<DataPortal<T>>();
        }

        public IServiceScope GetScope()
        {
            return provider.CreateScope();
        }
    }
}
