using Csla;
using Csla6ModelTemplates.Contracts.Tree.View;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Tree.View
{
    /// <summary>
    /// Represents a read-only folder node collection.
    /// </summary>
    [Serializable]
    public class FolderNodeList : ReadOnlyList<FolderNodeList, FolderNode>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(FolderNodeList),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Data Access

        [FetchChild]
        private void Fetch(
            List<FolderNodeDao> list,
            [Inject] IChildDataPortal<FolderNode> childPortal
            )
        {
            using (LoadListMode)
            {
                foreach (var item in list)
                    Add(childPortal.FetchChild(item));
            }
        }

        #endregion
    }
}
