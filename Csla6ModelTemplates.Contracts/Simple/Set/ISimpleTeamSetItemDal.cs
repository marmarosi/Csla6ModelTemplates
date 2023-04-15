namespace Csla6ModelTemplates.Contracts.Simple.Set
{
    /// <summary>
    /// Defines the data access functions of the editable team object.
    /// </summary>
    public interface ISimpleTeamSetItemDal
    {
        void Insert(SimpleTeamSetItemDao dao);
        void Update(SimpleTeamSetItemDao dao);
        void Delete(SimpleTeamSetItemCriteria criteria);
    }
}
