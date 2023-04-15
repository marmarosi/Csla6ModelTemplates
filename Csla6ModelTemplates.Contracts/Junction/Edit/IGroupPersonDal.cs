namespace Csla6ModelTemplates.Contracts.Junction.Edit
{
    /// <summary>
    /// Defines the data access functions of the editable group-person object.
    /// </summary>
    public interface IGroupPersonDal
    {
        void Insert(GroupPersonDao dao);
        void Delete(GroupPersonDao dao);
    }
}
