namespace Csla6ModelTemplates.Contracts.Complex.List
{
    /// <summary>
    /// Defines the read-only player list item data.
    /// </summary>
    public class PlayerListItemData
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only player list item object.
    /// </summary>
    public class PlayerListItemDao : PlayerListItemData
    {
        public long? PlayerKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only player list item object.
    /// </summary>
    public class PlayerListItemDto : PlayerListItemData
    {
        public string PlayerId { get; set; }
    }
}
