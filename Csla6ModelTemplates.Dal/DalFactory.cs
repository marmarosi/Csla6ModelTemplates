using Csla6ModelTemplates.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Specialized;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Factory of the CSLA data access objects.
    /// </summary>
    public static class DalFactory
    {
        private static StringDictionary Connections = new StringDictionary();
        private static Dictionary<string, Type> DalTypes = new Dictionary<string, Type>();
        public static string ActiveLayer;

        /// <summary>
        /// The name of the configuration section holding the DAL manager types.
        /// </summary>
        public const string Section = "DalSettings";

        #region Configure

        /// <summary>
        /// Configures and registers the data access layers.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="services">The container of the application services.</param>
        /// <param name="settings">The data access layer settings.</param>
        public static void Configure(
            IConfiguration configuration,
            IServiceCollection services
            )
        {
            DalSettings settings = configuration.GetSection(Section).Get<DalSettings>();
            Configure(settings);

            foreach (KeyValuePair<string, Type> dalType in DalTypes)
            {
                IDalManager dalManager = Activator.CreateInstance(dalType.Value) as IDalManager;
                dalManager.AddDalContext(configuration, services);
            }
        }

        /// <summary>
        /// Configures the data access layers.
        /// </summary>
        /// <param name="settings">The data access layer settings.</param>
        public static void Configure(
            DalSettings settings
            )
        {
            if (settings.Layers.Count == 0)
                throw new ArgumentException(CommonText.DalFactory_DalManager_NoDatabases);

            foreach (KeyValuePair<string, LayerSettings> entry in settings.Layers)
            {
                if (string.IsNullOrEmpty(entry.Value.ConnectionString))
                    throw new NullReferenceException(CommonText.DalFactory_DalManager_NoConnStr.With(entry.Value));

                Connections.Add(entry.Key, entry.Value.ConnectionString);
                ResolveDalType(entry.Key, entry.Value.DalManagerType);

                if (ActiveLayer == null)
                    ActiveLayer = entry.Key;
            }
            if (!string.IsNullOrWhiteSpace(settings.ActiveLayer))
            {
                if (!DalTypes.ContainsKey(settings.ActiveLayer))
                    throw new ArgumentException(CommonText.DalFactory_DalManager_WrongKey.With(settings.ActiveLayer));

                ActiveLayer = settings.ActiveLayer;
            }
        }

        private static void ResolveDalType(
            string dalName,
            string dalTypeName
            )
        {
            Type dalType = null;

            if (!string.IsNullOrEmpty(dalTypeName))
                dalType = Type.GetType(dalTypeName);
            else
                throw new NullReferenceException(CommonText.DalFactory_DalManager_NoDalMngr.With(dalName));

            if (dalType == null)
                throw new ArgumentException(CommonText.DalFactory_DalManager_NotFound.With(dalTypeName));

            DalTypes.Add(dalName, dalType);
        }

        #endregion

        #region Managers

        /// <summary>
        /// Gets the data access manager of the active layer.
        /// </summary>
        /// <returns>The data access manager object.</returns>
        public static IDalManager GetManager()
        {
            return GetManager(ActiveLayer);
        }

        /// <summary>
        /// Gets the data access manager with the specified name.
        /// </summary>
        /// <param name="dalName">The name of the data access layer.</param>
        /// <returns>The data access manager object.</returns>
        public static IDalManager GetManager(
            string dalName
            )
        {
            return Activator.CreateInstance(DalTypes[dalName]) as IDalManager;
        }

        /// <summary>
        /// CHecks whether the reason of the exception is a deadlock.
        /// </summary>
        /// <param name="ex">The original exception thrown.</param>
        /// <returns>True when the reason is a deadlock; otherwise false;</returns>
        public static bool HasDeadlock(Exception ex)
        {
            IDalManager dalManager = GetManager();
            if (dalManager.HasDeadlock(ex))
                return true;

            foreach (KeyValuePair<string, Type> dalType in DalTypes)
            {
                if (dalType.Key == ActiveLayer)
                    continue;
                dalManager = GetManager(dalType.Key);
                if (dalManager.HasDeadlock(ex))
                    return true;
            }
            return false;
        }

        #endregion

        #region ConnectionString

        /// <summary>
        /// Gets the connection string with the specified name.
        /// </summary>
        /// <param name="dalName">The name of the data access layer.</param>
        /// <returns>The connection string.</returns>
        public static string GetConnectionString(
            string dalName
            )
        {
            return Connections[dalName];
        }

        #endregion

        #region ProductionSeeds

        /// <summary>
        /// Ensures the initial data of the active layer.
        /// </summary>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void SeedProductionData(
            string contentRootPath
            )
        {
            SeedProductionData(contentRootPath, ActiveLayer);
        }

        /// <summary>
        /// Ensures the initial data of the specified layer.
        /// </summary>
        /// <param name="contentRootPath">The root path of the web site.</param>
        /// <param name="dalName">The name of the data access layer.</param>
        public static void SeedProductionData(
            string contentRootPath,
            string dalName
            )
        {
            ISeeder seeder = Activator.CreateInstance(DalTypes[dalName]) as ISeeder;
            seeder.SeedProductionData(contentRootPath);
        }

        /// <summary>
        /// Ensures the initial data of all layers.
        /// </summary>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void SeedProductionDataForAll(
            string contentRootPath
            )
        {
            foreach (KeyValuePair<string, Type> dalType in DalTypes)
                SeedProductionData(dalType.Key, contentRootPath);
        }

        #endregion

        #region DevelopmentSeeds

        /// <summary>
        /// Ensures the demo data of the active layer.
        /// </summary>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void SeedDevelopmentData(
            string contentRootPath
            )
        {
            SeedDevelopmentData(contentRootPath, ActiveLayer);
        }

        /// <summary>
        /// Ensures the demo data of the specified layer.
        /// </summary>
        /// <param name="contentRootPath">The root path of the web site.</param>
        /// <param name="dalName">The name of the data access layer.</param>
        public static void SeedDevelopmentData(
            string contentRootPath,
            string dalName
            )
        {
            ISeeder seeder = Activator.CreateInstance(DalTypes[dalName ?? ActiveLayer]) as ISeeder;
            seeder.SeedDevelopmentData(contentRootPath);
        }

        /// <summary>
        /// Ensures the demo data of all layers.
        /// </summary>
        /// <param name="contentRootPath">The root path of the web site.</param>
        public static void SeedDevelopmentDataForAll(
            string contentRootPath
            )
        {
            foreach (KeyValuePair<string, Type> dalType in DalTypes)
                SeedDevelopmentData(contentRootPath, dalType.Key);
        }

        #endregion
    }
}
