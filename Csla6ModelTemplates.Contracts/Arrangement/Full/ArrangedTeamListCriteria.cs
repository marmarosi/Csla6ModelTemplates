using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Arrangement.Full
{
    /// <summary>
    /// Represents the criteria of the read-only paginated sorted team collection.
    /// </summary>
    [Serializable]
    public class ArrangedTeamListCriteria : PaginatedSortedListCriteria
    {
        public string TeamName { get; set; }
    }
}
