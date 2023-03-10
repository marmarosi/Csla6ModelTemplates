using Csla6ModelTemplates.Dal;

namespace Csla6ModelTemplates.Contracts.Simple.Edit
{
    /// <summary>
    /// Defines the data access functions of the editable team object.
    /// </summary>
    public interface ISimpleTeamDal : ITransactionalDal
    {
        SimpleTeamDao Fetch(SimpleTeamCriteria criteria);
        void Insert(SimpleTeamDao dao);
        void Update(SimpleTeamDao dao);
        void Delete(SimpleTeamCriteria criteria);
    }
}
