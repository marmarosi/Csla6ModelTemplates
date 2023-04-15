using Csla6ModelTemplates.Dal;

namespace Csla6ModelTemplates.Contracts.Junction.Edit
{
    /// <summary>
    /// Defines the data access functions of the editable group object.
    /// </summary>
    public interface IGroupDal : ITransactionalDal
    {
        GroupDao Fetch(GroupCriteria criteria);
        void Insert(GroupDao dao);
        void Update(GroupDao dao);
        void Delete(GroupCriteria criteria);
    }
}
