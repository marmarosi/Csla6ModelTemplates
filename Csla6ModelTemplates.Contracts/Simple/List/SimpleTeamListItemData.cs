namespace Csla6ModelTemplates.Contracts.Simple.List
{
    /// <summary>
    /// Defines the read-only team list item data.
    /// </summary>
    public class SimpleTeamListItemData
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only team list item model.
    /// </summary>
    public class SimpleTeamListItemDao : SimpleTeamListItemData
    {
        public long? TeamKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only team list item model.
    /// </summary>
    public class SimpleTeamListItemDto : SimpleTeamListItemData
    {
        public string TeamId { get; set; }
    }
}
