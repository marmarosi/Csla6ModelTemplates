using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Selection.WithKey
{
    /// <summary>
    /// Defines the data access functions of the read-only team choice collection.
    /// </summary>
    public interface ITeamKeyChoiceDal : IKeyNameChoiceDal<TeamKeyChoiceCriteria>
    {
        new List<KeyNameOptionDao> Fetch(TeamKeyChoiceCriteria criteria);
    }
}
