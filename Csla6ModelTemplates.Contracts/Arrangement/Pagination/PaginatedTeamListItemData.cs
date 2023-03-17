namespace Csla6ModelTemplates.Contracts.Arrangement.Pagination
{
    /// <summary>
    /// Defines the read-only paginated team list item data.
    /// </summary>
    public class PaginatedTeamListItemData
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only paginated team list item object.
    /// </summary>
    public class PaginatedTeamListItemDao : PaginatedTeamListItemData
    {
        public long? TeamKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only paginated team list item object.
    /// </summary>
    public class PaginatedTeamListItemDto : PaginatedTeamListItemData
    {
        public string TeamId { get; set; }
    }
}
