using Csla;
using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Complex.List
{
    /// <summary>
    /// Represents a read-only player info collection.
    /// </summary>
    [Serializable]
    public class PlayerInfoss : ReadOnlyList<PlayerInfoss, PlayerInfo>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(PlayerInfos),
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
            List<PlayerInfoDao> list,
            [Inject] IChildDataPortal<PlayerInfo> itemPortal
            )
        {
            // Load values from persistent storage.
            foreach (PlayerInfoDao item in list)
                Items.Add(itemPortal.FetchChild(item));
        }

        #endregion
    }
}
