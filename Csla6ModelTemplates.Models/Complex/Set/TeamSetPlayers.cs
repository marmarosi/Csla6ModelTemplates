using Csla;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.CslaExtensions;

namespace Csla6ModelTemplates.Models.Complex.Set
{
    /// <summary>
    /// Represents an editable player collection.
    /// </summary>
    [Serializable]
    public class TeamSetPlayers : EditableList<TeamSetPlayers, TeamSetPlayer, TeamSetPlayerDto>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(TeamSetPlayers),
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
            List<TeamSetPlayerDao> list,
            [Inject] IChildDataPortal<TeamSetPlayer> childPortal
            )
        {
            foreach (var item in list)
                Add(childPortal.FetchChild(item));
        }

        [UpdateChild]
        protected void Update()
        {
            // Update values in persistent storage.
            Child_Update();
        }

        #endregion
    }
}
