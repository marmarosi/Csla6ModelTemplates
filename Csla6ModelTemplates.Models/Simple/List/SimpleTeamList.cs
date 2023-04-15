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
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets a list of teams that matches the criteria.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <returns>The requested team list.</returns>
        public static async Task<SimpleTeamList> Get(
            IDataPortalFactory factory,
            SimpleTeamListCriteria criteria
            )
        {
            return await factory.GetPortal<SimpleTeamList>().FetchAsync(criteria);
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            SimpleTeamListCriteria criteria,
            [Inject] ISimpleTeamListDal dal,
            [Inject] IChildDataPortal<SimpleTeamListItem> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<SimpleTeamListItemDao> list = dal.Fetch(criteria);
                foreach (var item in list)
                    Add(itemPortal.FetchChild(item));
            }
        }

        #endregion
    }
}
