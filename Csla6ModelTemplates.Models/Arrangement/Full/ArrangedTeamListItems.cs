using Csla;
using Csla6ModelTemplates.Contracts.Arrangement.Full;
using Csla6ModelTemplates.CslaExtensions;

namespace Csla6ModelTemplates.Models.Arrangement.Full
{
    /// <summary>
    /// Represents a page of read-only paginated sorted team collection.
    /// </summary>
    [Serializable]
    public class ArrangedTeamListItems : ReadOnlyList<ArrangedTeamListItems, ArrangedTeamListItem>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(ArrangedTeamListItems),
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
            List<ArrangedTeamListItemDao> list,
            [Inject] IChildDataPortal<ArrangedTeamListItem> itemPortal
            )
        {
            // Load values from persistent storage.
            foreach (var item in list)
                Items.Add(itemPortal.FetchChild(item));
        }

        #endregion
    }
}
