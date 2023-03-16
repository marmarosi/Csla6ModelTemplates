namespace Csla6ModelTemplates.Contracts.Complex.List
{
    /// <summary>
    /// Defines the read-only player list item data.
    /// </summary>
    public class PlayerInfoData
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only player info object.
    /// </summary>
    public class PlayerInfoDao : PlayerInfoData
    {
        public long? PlayerKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only player info object.
    /// </summary>
    public class PlayerInfoDto : PlayerInfoData
    {
        public string PlayerId { get; set; }
    }
}
