using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Selection.WithId
{
    /// <summary>
    /// Defines the data access functions of the read-only team choice collection.
    /// </summary>
    public interface ITeamIdChoiceDal : IIdNameChoiceDal<TeamIdChoiceCriteria>
    {
        new List<IdNameOptionDao> Fetch(TeamIdChoiceCriteria criteria);
    }
}
