using System.Reflection;

namespace Csla6ModelTemplates.Endpoints.Extensions
{
    /// <summary>
    /// Provide methods to build the application configuration.
    /// </summary>
    internal static class ConfigExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">Context containing the common services on the IHost.</param>
        /// <param name="configBuilder">The application configuration builder.</param>
        public static void Build(
            HostBuilderContext context,
            IConfigurationBuilder configBuilder
            )
        {
            var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var webRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var basePath = Path.Join(webRootPath, "../../..");
            var sharedSettings = Path.Join(basePath, "../Shared/SharedSettings.json");

            //configBuilder.SetBasePath(basePath);
            configBuilder.AddJsonFile(sharedSettings, false, true);
            configBuilder.AddJsonFile("appsettings.json", false, true);
            configBuilder.AddJsonFile($"appsettings.{enviroment}.json", true, true);
            //configBuilder.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true);

            configBuilder.AddEnvironmentVariables();
        }
    }
}
