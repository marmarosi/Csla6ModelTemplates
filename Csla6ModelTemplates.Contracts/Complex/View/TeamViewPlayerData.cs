namespace Csla6ModelTemplates.Contracts.Complex.View
{
    /// <summary>
    /// Defines the read-only player data.
    /// </summary>
    public class TeamViewPlayerData
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only player object.
    /// </summary>
    public class TeamViewPlayerDao : TeamViewPlayerData
    {
        public long? PlayerKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only player object.
    /// </summary>
    public class TeamViewPlayerDto : TeamViewPlayerData
    {
        public string PlayerId { get; set; }
    }
}
