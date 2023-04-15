using Csla;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Tree.Choice;
using Csla6ModelTemplates.CslaExtensions.Models;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Tree.Choice
{
    /// <summary>
    /// Represents a read-only tree choice collection.
    /// </summary>
    [Serializable]
    public class RootFolderChoice : ReadOnlyList<RootFolderChoice, IdNameOption>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(TeamIdChoice),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets a choice of tree options.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <returns>The requested tree choice instance.</returns>
        public static async Task<RootFolderChoice> Get(
            IDataPortalFactory factory
            )
        {
            var criteria = new RootFolderChoiceCriteria();
            return await factory.GetPortal<RootFolderChoice>().FetchAsync(criteria);
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            RootFolderChoiceCriteria criteria,
            [Inject] IRootFolderChoiceDal dal,
            [Inject] IChildDataPortal<IdNameOption> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<IdNameOptionDao> list = dal.Fetch(criteria);
                foreach (var item in list)
                    Add(itemPortal.FetchChild(item, ID.Folder));
            }
        }

        #endregion
    }
}
