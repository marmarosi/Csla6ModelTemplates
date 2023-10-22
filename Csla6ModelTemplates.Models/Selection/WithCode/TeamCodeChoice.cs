using Csla;
using Csla6ModelTemplates.Contracts.Selection.WithCode;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Selection.WithCode
{
    /// <summary>
    /// Represents a read-only team choice collection.
    /// </summary>
    [Serializable]
    public class TeamCodeChoice : ReadOnlyList<TeamCodeChoice, CodeNameOption>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(TeamCodeChoice),
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
        public static async Task<TeamCodeChoice> Get(
            IDataPortalFactory factory,
            TeamCodeChoiceCriteria criteria
            )
        {
            return await factory.GetPortal<TeamCodeChoice>().FetchAsync(criteria);
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            TeamCodeChoiceCriteria criteria,
            [Inject] ITeamCodeChoiceDal dal,
            [Inject] IChildDataPortal<CodeNameOption> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<CodeNameOptionDao> list = dal.Fetch(criteria);
                foreach (var item in list)
                    Add(itemPortal.FetchChild(item));
            }
        }

        #endregion
    }
}
