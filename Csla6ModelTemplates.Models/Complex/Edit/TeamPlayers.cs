﻿using Csla;
using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.CslaExtensions;

namespace Csla6ModelTemplates.Models.Complex.Edit
{
    /// <summary>
    /// Represents an editable player collection.
    /// </summary>
    [Serializable]
    public class TeamPlayers : EditableList<TeamPlayers, TeamPlayer, TeamPlayerDto>
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
            List<TeamPlayerDao> list,
            [Inject] IChildDataPortal<TeamPlayer> itemPortal
            )
        {
            foreach (var item in list)
                Add(itemPortal.FetchChild(item));
        }

        #endregion
    }
}
