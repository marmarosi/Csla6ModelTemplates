using Csla;
using Csla.Configuration;
using Csla.DataPortalClient;
using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Dal.SqlServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1)]
namespace Csla6ModelTemplates.WebApiTests
{
    internal class SetupService
    {
        private static readonly SetupService _setupServiceInstance = new SetupService();
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();
        private readonly IServiceProvider _serviceProvider;
        //private readonly IDataPortalFactory _dataPortalFactory;

        private SetupService()
        {
            // Get the configuration.
            IConfiguration configuration = GetConfig();

            // Initializes a new instance of ServiceProvider class.
            _serviceProvider = _serviceCollection.BuildServiceProvider();

            // Configure CSLA.
            _serviceCollection.AddHttpContextAccessor();
            _serviceCollection.AddCsla(o => o
                .AddAspNetCore()
                .DataPortal(dpo => dpo
                    .UseLocalProxy(options => {
                        options.UseLocalScope = true;
                        options.FlowSynchronizationContext = false;
                    })
                )
            );
            //_dataPortalFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();

            //// Configure data access layers.
            //DalFactory.Configure(configuration, _serviceCollection);
            //if (DalFactory.ActiveLayer == DAL.SQLite)
            //    DalFactory.SeedDevelopmentData(null);

            // Configure data access layer.
            var dalIndex = new SqlServerDalIndex();
            var dalTypes = dalIndex.GetDalItems();
            foreach (var dalType in dalTypes)
            {
                _serviceCollection.AddScoped(dalType.Key, dalType.Value);
            }
        }

        public static SetupService GetInstance()
        {
            return _setupServiceInstance;
        }

        public IDataPortal<T> GetPortal<T>() where T: class
        {
            var _dataPortalFactory = _serviceProvider.GetRequiredService<IDataPortalFactory>();
            return _dataPortalFactory.GetPortal<T>();
        }

        private IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("SharedSettings.json", true, true)
                .AddJsonFile("appsettings.json", true, true) // use appsettings.json in current folder
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public ILogger<T> GetLogger<T>() where T : class
        {
            // Create logger.
            return new NullLogger<T>();
        }

        public IServiceScope GetScope()
        {
            return _serviceProvider.CreateScope();
        }
    }
}
