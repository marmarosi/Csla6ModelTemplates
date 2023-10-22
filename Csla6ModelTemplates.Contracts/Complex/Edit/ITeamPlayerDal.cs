namespace Csla6ModelTemplates.Contracts.Complex.Edit
{
    /// <summary>
    /// Defines the data access functions of the editable player object.
    /// </summary>
    public interface ITeamPlayerDal
    {
        void Insert(TeamPlayerDao dao);
        void Update(TeamPlayerDao dao);
        void Delete(TeamPlayerCriteria criteria);
    }
}
