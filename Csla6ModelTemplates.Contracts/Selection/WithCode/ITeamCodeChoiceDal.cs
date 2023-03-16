using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Selection.WithCode
{
    /// <summary>
    /// Defines the data access functions of the read-only team choice collection.
    /// </summary>
    public interface ITeamCodeChoiceDal : ICodeNameChoiceDal<TeamCodeChoiceCriteria>
    {
        new List<CodeNameOptionDao> Fetch(TeamCodeChoiceCriteria criteria);
    }
}
