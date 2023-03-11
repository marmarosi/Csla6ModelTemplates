namespace Csla6ModelTemplates.Contracts.Complex.View
{
    /// <summary>
    /// Defines the data access functions of the read-only team object.
    /// </summary>
    public interface ITeamViewDal
    {
        TeamViewDao Fetch(TeamViewCriteria criteria);
    }
}
