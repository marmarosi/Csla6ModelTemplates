using Csla;
using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Complex.List
{
    /// <summary>
    /// Represents a read-only team collection.
    /// </summary>
    [Serializable]
    public class TeamList : ReadOnlyList<TeamList, TeamListItem>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(TeamList),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            TeamListCriteria criteria,
            [Inject] ITeamListDal dal,
            [Inject] IChildDataPortal<TeamListItem> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<TeamListItemDao> list = dal.Fetch(criteria);
                foreach (var item in list)
                    Add(itemPortal.FetchChild(item));
            }
        }

        #endregion
    }
}
