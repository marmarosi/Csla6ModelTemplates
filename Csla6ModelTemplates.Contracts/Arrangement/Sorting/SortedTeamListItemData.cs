namespace Csla6ModelTemplates.Contracts.Arrangement.Sorting
{
    /// <summary>
    /// Defines the read-only sorted team list item data.
    /// </summary>
    public class SortedTeamListItemData
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only sorted team list item object.
    /// </summary>
    public class SortedTeamListItemDao : SortedTeamListItemData
    {
        public long? TeamKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only sorted team list item object.
    /// </summary>
    public class SortedTeamListItemDto : SortedTeamListItemData
    {
        public string TeamId { get; set; }
    }
}
