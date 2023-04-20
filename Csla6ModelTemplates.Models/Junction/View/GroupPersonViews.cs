using Csla;
using Csla6ModelTemplates.Contracts.Junction.View;
using Csla6ModelTemplates.CslaExtensions;

namespace Csla6ModelTemplates.Models.Junction.View
{
    /// <summary>
    /// Represents a read-only group-person collection.
    /// </summary>
    [Serializable]
    public class GroupPersonViews : ReadOnlyList<GroupPersonViews, GroupPersonView>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(GroupPersonViews),
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
            List<GroupPersonViewDao> list,
            [Inject] IChildDataPortal<GroupPersonView> itemPortal
            )
        {
            // Load values from persistent storage.
            foreach (var item in list)
                Items.Add(itemPortal.FetchChild(item));
        }

        #endregion
    }
}
