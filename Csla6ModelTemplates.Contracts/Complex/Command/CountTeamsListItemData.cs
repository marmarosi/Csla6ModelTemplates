namespace Csla6ModelTemplates.Contracts.Complex.Command
{
    /// <summary>
    /// Defines the count teams list item data.
    /// </summary>
    public class CountTeamsListItemData
    {
        public int ItemCount { get; set; }
        public int CountOfTeams { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the count teams list item object.
    /// </summary>
    public class CountTeamsListItemDao : CountTeamsListItemData
    { }

    /// <summary>
    /// Defines the data transfer object of the count teams list item object.
    /// </summary>
    public class CountTeamsListItemDto : CountTeamsListItemData
    { }
}
