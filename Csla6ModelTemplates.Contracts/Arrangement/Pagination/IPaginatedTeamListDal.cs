using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Arrangement.Pagination
{
    /// <summary>
    /// Defines the data access functions of the read-only paginated team collection.
    /// </summary>
    public interface IPaginatedTeamListDal
    {
        IPaginatedList<PaginatedTeamListItemDao> Fetch(PaginatedTeamListCriteria criteria);
    }
}
