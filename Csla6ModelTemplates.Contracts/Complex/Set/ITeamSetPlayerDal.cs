namespace Csla6ModelTemplates.Contracts.Complex.Set
{
    /// <summary>
    /// Defines the data access functions of the editable player object.
    /// </summary>
    public interface ITeamSetPlayerDal
    {
        void Insert(TeamSetPlayerDao dao);
        void Update(TeamSetPlayerDao dao);
        void Delete(TeamSetPlayerCriteria criteria);
    }
}
