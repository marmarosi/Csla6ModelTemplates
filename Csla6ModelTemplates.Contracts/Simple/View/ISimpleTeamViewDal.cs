namespace Csla6ModelTemplates.Contracts.Simple.View
{
    /// <summary>
    /// Defines the data access functions of the read-only team model.
    /// </summary>
    public interface ISimpleTeamViewDal
    {
        SimpleTeamViewDao GetView(SimpleTeamViewCriteria criteria);
    }
}
