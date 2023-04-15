using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Arrangement.Full
{
    /// <summary>
    /// Defines the data access functions of the read-only paginated sorted  team collection.
    /// </summary>
    public interface IArrangedTeamListDal
    {
        IPaginatedList<ArrangedTeamListItemDao> Fetch(ArrangedTeamListCriteria criteria);
    }
}
