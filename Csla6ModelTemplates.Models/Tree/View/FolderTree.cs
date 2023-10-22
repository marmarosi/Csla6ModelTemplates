using Csla;
using Csla6ModelTemplates.Contracts.Tree.View;
using Csla6ModelTemplates.CslaExtensions;

namespace Csla6ModelTemplates.Models.Tree.View
{
    /// <summary>
    /// Represents a read-only folder tree.
    /// </summary>
    [Serializable]
    public class FolderTree : ReadOnlyList<FolderTree, FolderNode>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(FolderTree),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets the specified read-only folder tree.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="criteria">The criteria of the read-only folder tree.</param>
        /// <returns>The requested read-only folder tree.</returns>
        public static async Task<FolderTree> Get(
            IDataPortalFactory factory,
            string rootId
            )
        {
            var criteria = new FolderTreeParams(rootId);
            return await factory.GetPortal<FolderTree>().FetchAsync(criteria.Decode());
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            FolderTreeCriteria criteria,
            [Inject] IFolderTreeDal dal,
            [Inject] IChildDataPortal<FolderNode> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<FolderNodeDao> list = dal.Fetch(criteria);
                foreach (var item in list)
                    Add(itemPortal.FetchChild(item));
            }
        }

        #endregion
    }
}
