using Csla6ModelTemplates.Dal;

namespace Csla6ModelTemplates.Contracts.Complex.Set
{
    /// <summary>
    /// Defines the data access functions of the editable team collection.
    /// </summary>
    public interface ITeamSetDal : ITransactionalDal
    {
        List<TeamSetItemDao> Fetch(TeamSetCriteria criteria);
    }
}
