//using Csla;
//using Csla6ModelTemplates.Dal.SqlServer;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
//using System;
//using System.Collections.Generic;

//namespace Csla6ModelTemplates.WebApiTests
//{
//    internal class TestApplication : WebApplicationFactory<Program>
//    {
//        protected override IHost CreateHost(IHostBuilder builder)
//        {
//            builder.ConfigureServices(s => {

//                // Configure data access layer.
//                var dalIndex = new SqlServerDalIndex();
//                var dalTypes = dalIndex.GetDalItems();
//                foreach (var dalType in dalTypes)
//                {
//                    s.AddScoped(dalType.Key, dalType.Value);
//                }
//            });

//            return base.CreateHost(builder);
//        }

//        public ILogger<T> GetLogger<T>() where T : class
//        {
//            // Create logger.
//            return new NullLogger<T>();
//        }

//        public IDataPortal<T> GetPortal<T>(
//            [FromServices] IDataPortal<T> portal
//            ) where T : class
//        {
//            return portal;
//        }
//    }
//}
