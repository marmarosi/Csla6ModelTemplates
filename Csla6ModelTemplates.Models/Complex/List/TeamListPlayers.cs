using Csla;
using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.CslaExtensions;

namespace Csla6ModelTemplates.Models.Complex.List
{
    /// <summary>
    /// Represents a read-only player info collection.
    /// </summary>
    [Serializable]
    public class TeamListPlayers : ReadOnlyList<TeamListPlayers, TeamListPlayer>
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
            List<TeamListPlayerDao> list,
            [Inject] IChildDataPortal<TeamListPlayer> itemPortal
            )
        {
            // Load values from persistent storage.
            foreach (var item in list)
                Items.Add(itemPortal.FetchChild(item));
        }

        #endregion
    }
}
