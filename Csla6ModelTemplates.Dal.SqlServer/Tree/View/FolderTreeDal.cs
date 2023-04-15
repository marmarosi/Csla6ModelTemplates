using Csla6ModelTemplates.Contracts.Tree.View;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.SqlServer.Tree.View
{
    /// <summary>
    /// Implements the data access functions of the read-only folder tree.
    /// </summary>
    [DalImplementation]
    public class FolderTreeDal : DalBase<SqlServerContext>, IFolderTreeDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public FolderTreeDal(
            SqlServerContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Fetch

        private List<FolderNodeDao> AllFolders { get; set; }

        /// <summary>
        /// Gets the specified folder tree.
        /// </summary>
        /// <param name="criteria">The criteria of the folder tree.</param>
        /// <returns>The requested folder tree.</returns>
        public List<FolderNodeDao> Fetch(
            FolderTreeCriteria criteria
            )
        {
            var tree = new List<FolderNodeDao>();

            // Get all subfolders of the root foolder.
            AllFolders = DbContext.Folders
                .Where(e =>
                    e.RootKey == criteria.RootKey
                )
                .Select(e => new FolderNodeDao
                {
                    FolderKey = e.FolderKey,
                    ParentKey = e.ParentKey,
                    FolderOrder = e.FolderOrder,
                    FolderName = e.FolderName
                })
                .AsNoTracking()
                .ToList();

            // Populate the tree.
            PopulateLevel(1, null, tree);

            // Return the result.
            return tree;
        }

        private void PopulateLevel(
            int level,
            long? parentKey,
            List<FolderNodeDao> parentChildren
            )
        {
            // Get the folders of the level.
            var folders = AllFolders
                .Where(o => o.ParentKey == parentKey)
                .OrderBy(o => o.FolderOrder)
                .ToList();

            foreach (FolderNodeDao folder in folders)
            {
                // Create folder node.
                FolderNodeDao folderNode = new FolderNodeDao
                {
                    FolderKey = folder.FolderKey,
                    ParentKey = folder.ParentKey,
                    FolderOrder = folder.FolderOrder,
                    FolderName = folder.FolderName,
                    Level = level,
                    Children = new List<FolderNodeDao>()
                };

                // Add folder to the parent's children.
                parentChildren.Add(folderNode);

                // Get the subfolders of this folder.
                PopulateLevel(
                    level + 1,
                    folder.FolderKey,
                    folderNode.Children
                    );
            }
        }

        #endregion GetList
    }
}
