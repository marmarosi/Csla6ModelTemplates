using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Tree.Choice
{
    /// <summary>
    /// Defines the data access functions of the read-only tree choice collection.
    /// </summary>
    public interface IRootFolderChoiceDal : IIdNameChoiceDal<RootFolderChoiceCriteria>
    {
        new List<IdNameOptionDao> Fetch(RootFolderChoiceCriteria criteria);
    }
}
