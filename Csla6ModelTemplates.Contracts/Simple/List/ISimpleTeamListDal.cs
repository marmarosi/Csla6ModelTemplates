namespace Csla6ModelTemplates.Contracts.Simple.List
{
    /// <summary>
    /// Defines the data access functions of the read-only team collection.
    /// </summary>
    public interface ISimpleTeamListDal
    {
        List<SimpleTeamListItemDao> Fetch(SimpleTeamListCriteria criteria);
    }
}
