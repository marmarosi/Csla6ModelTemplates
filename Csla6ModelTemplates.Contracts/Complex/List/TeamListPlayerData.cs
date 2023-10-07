namespace Csla6ModelTemplates.Contracts.Complex.List
{
    /// <summary>
    /// Defines the read-only player list item data.
    /// </summary>
    public class TeamListPlayerData
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only player info object.
    /// </summary>
    public class TeamListPlayerDao : TeamListPlayerData
    {
        public long? PlayerKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only player info object.
    /// </summary>
    public class TeamListPlayerDto : TeamListPlayerData
    {
        public string PlayerId { get; set; }
    }
}
