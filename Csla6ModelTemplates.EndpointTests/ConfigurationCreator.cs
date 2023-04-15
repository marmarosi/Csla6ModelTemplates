using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Csla6ModelTemplates.EndpointTests
{
    /// <summary>
    /// Provide methods to create the application configuration.
    /// </summary>
    internal static class ConfigurationCreator
    {
        /// <summary>
        /// Creates the application configuration.
        /// </summary>
        /// <returns>The application configuration.</returns>
        public static IConfiguration Create()
        {
            var webRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var basePath = Path.Join(webRootPath, "../../..");
            var sharedSettings = Path.Join(basePath, "../Shared/SharedSettings.json");

            var builder = new ConfigurationBuilder()
                .AddJsonFile(sharedSettings, true, true)
                .AddEnvironmentVariables();

            IConfiguration configuration = builder.Build();
            return configuration;
        }
    }
}
