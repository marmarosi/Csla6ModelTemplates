using Csla;
using Csla6ModelTemplates.Contracts.Complex.View;
using Csla6ModelTemplates.CslaExtensions;

namespace Csla6ModelTemplates.Models.Complex.View
{
    /// <summary>
    /// Represents a read-only player collection.
    /// </summary>
    [Serializable]
    public class PlayerViews : ReadOnlyList<PlayerViews, PlayerView>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(PlayerViews),
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
            List<PlayerViewDao> list,
            [Inject] IChildDataPortal<PlayerView> itemPortal
            )
        {
            // Load values from persistent storage.
            foreach (var item in list)
                Items.Add(itemPortal.FetchChild(item));
        }

        #endregion
    }
}
