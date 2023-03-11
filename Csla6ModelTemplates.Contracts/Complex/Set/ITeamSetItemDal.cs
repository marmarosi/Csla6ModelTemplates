namespace Csla6ModelTemplates.Contracts.Complex.Set
{
    /// <summary>
    /// Defines the data access functions of the editable team set item object.
    /// </summary>
    public interface ITeamSetItemDal
    {
        void Insert(TeamSetItemDao dao);
        void Update(TeamSetItemDao dao);
        void Delete(TeamSetItemCriteria criteria);
    }
}
