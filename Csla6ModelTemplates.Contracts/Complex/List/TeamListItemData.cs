namespace Csla6ModelTemplates.Contracts.Complex.List
{
    /// <summary>
    /// Defines the read-only team list item data.
    /// </summary>
    public class TeamListItemData
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only team list item object.
    /// </summary>
    public class TeamListItemDao : TeamListItemData
    {
        public long? TeamKey { get; set; }
        public List<PlayerListItemDao> Players { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only team list item object.
    /// </summary>
    public class TeamListItemDto : TeamListItemData
    {
        public string TeamId { get; set; }
        public List<PlayerListItemDto> Players { get; set; }
    }
}
