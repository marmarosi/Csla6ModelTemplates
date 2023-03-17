using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Arrangement.Pagination
{
    /// <summary>
    /// Represents the criteria of the read-only paginated team collection.
    /// </summary>
    [Serializable]
    public class PaginatedTeamListCriteria : PaginatedListCriteria
    {
        public string TeamName { get; set; }
    }
}
