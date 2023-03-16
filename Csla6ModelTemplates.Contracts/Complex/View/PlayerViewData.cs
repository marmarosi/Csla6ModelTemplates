namespace Csla6ModelTemplates.Contracts.Complex.View
{
    /// <summary>
    /// Defines the read-only player data.
    /// </summary>
    public class PlayerViewData
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only player object.
    /// </summary>
    public class PlayerViewDao : PlayerViewData
    {
        public long? PlayerKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only player object.
    /// </summary>
    public class PlayerViewDto : PlayerViewData
    {
        public string PlayerId { get; set; }
    }
}
