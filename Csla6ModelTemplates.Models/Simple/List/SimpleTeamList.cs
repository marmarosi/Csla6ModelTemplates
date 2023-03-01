using Csla;
using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Simple.List
{
    /// <summary>
    /// Represents a read-only team collection.
    /// </summary>
    [Serializable]
    public class SimpleTeamList : ReadOnlyList<SimpleTeamList, SimpleTeamListItem>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(SimpleTeamList),
        //        new IsInRole(AuthorizationActions.GetObject, "Manager")
        //        );
        //}

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            SimpleTeamListCriteria criteria,
            [Inject] ISimpleTeamListDal dal,
            [Inject] IChildDataPortal<SimpleTeamListItem> childPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<SimpleTeamListItemDao> list = dal.GetList(criteria);
                foreach (var item in list)
                    Add(childPortal.FetchChild(item));
            }
        }

        #endregion
    }
}
