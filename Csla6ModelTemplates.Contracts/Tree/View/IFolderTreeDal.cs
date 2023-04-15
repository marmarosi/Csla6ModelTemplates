namespace Csla6ModelTemplates.Contracts.Tree.View
{
    /// <summary>
    /// Defines the data access functions of the read-only folder tree object.
    /// </summary>
    public interface IFolderTreeDal
    {
        List<FolderNodeDao> Fetch(FolderTreeCriteria criteria);
    }
}
