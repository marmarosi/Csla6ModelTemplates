namespace Csla6ModelTemplates.Contracts.Arrangement.Full
{
    /// <summary>
    /// Defines the read-only paginated sorted team list item data.
    /// </summary>
    public class ArrangedTeamListItemData
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only paginated sorted team list item object.
    /// </summary>
    public class ArrangedTeamListItemDao : ArrangedTeamListItemData
    {
        public long? TeamKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only paginated sorted team list item object.
    /// </summary>
    public class ArrangedTeamListItemDto : ArrangedTeamListItemData
    {
        public string TeamId { get; set; }
    }
}
