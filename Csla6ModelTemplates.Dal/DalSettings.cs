namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Represents the configuration section for the data access layers.
    /// </summary>
    public class DalSettings
    {
        /// <summary>
        /// The list of the layer settings.
        /// </summary>
        public Dictionary<string, LayerSettings> Layers { get; set; }

        /// <summary>
        /// The key of the active layer.
        /// </summary>
        public string ActiveLayer { get; set; }
    }

    /// <summary>
    /// Represents the settings of a database.
    /// </summary>
    public class LayerSettings
    {
        /// <summary>
        /// The connection string of the layer.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The data access layer manager of the layer.
        /// </summary>
        public string DalManagerType { get; set; }
    }
}
