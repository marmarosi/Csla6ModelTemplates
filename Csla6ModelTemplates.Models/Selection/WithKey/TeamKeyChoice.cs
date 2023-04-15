using Csla;
using Csla6ModelTemplates.Contracts.Selection.WithKey;
using Csla6ModelTemplates.CslaExtensions.Models;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Selection.WithKey
{
    /// <summary>
    /// Represents a read-only team choice collection.
    /// </summary>
    [Serializable]
    public class TeamKeyChoice : ReadOnlyList<TeamKeyChoice, KeyNameOption>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(TeamKeyChoice),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets a choice of team options that match the criteria.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="criteria">The criteria team choice.</param>
        /// <returns>The requested team choice instance.</returns>
        public static async Task<TeamKeyChoice> Get(
            IDataPortalFactory factory,
            TeamKeyChoiceCriteria criteria
            )
        {
            return await factory.GetPortal<TeamKeyChoice>().FetchAsync(criteria);
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            TeamKeyChoiceCriteria criteria,
            [Inject] ITeamKeyChoiceDal dal,
            [Inject] IChildDataPortal<KeyNameOption> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<KeyNameOptionDao> list = dal.Fetch(criteria);
                foreach (var item in list)
                    Add(itemPortal.FetchChild(item));
            }
        }

        #endregion
    }
}
