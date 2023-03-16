using Csla;
using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Complex.Edit
{
    /// <summary>
    /// Represents an editable player collection.
    /// </summary>
    [Serializable]
    public class Players : EditableList<Players, Player, PlayerDto>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(Players),
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
            List<PlayerDao> list,
            [Inject] IChildDataPortal<Player> itemPortal
            )
        {
            foreach (PlayerDao item in list)
                Add(itemPortal.FetchChild(item));
        }

        #endregion
    }
}
