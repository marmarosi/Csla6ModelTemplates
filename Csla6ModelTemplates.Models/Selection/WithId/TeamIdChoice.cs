using Csla;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Selection.WithId;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Selection.WithId
{
    /// <summary>
    /// Represents a read-only team choice collection.
    /// </summary>
    [Serializable]
    public class TeamIdChoice : ReadOnlyList<TeamIdChoice, IdNameOption>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(TeamIdChoice),
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
        /// <param name="criteria">The criteria of the team choice.</param>
        /// <returns>The requested team choice instance.</returns>
        public static async Task<TeamIdChoice> Get(
            IDataPortalFactory factory,
            TeamIdChoiceCriteria criteria
            )
        {
            return await factory.GetPortal<TeamIdChoice>().FetchAsync(criteria);
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            TeamIdChoiceCriteria criteria,
            [Inject] ITeamIdChoiceDal dal,
            [Inject] IChildDataPortal<IdNameOption> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<IdNameOptionDao> list = dal.Fetch(criteria);
                foreach (var item in list)
                    Add(itemPortal.FetchChild(item, ID.Team));
            }
        }

        #endregion
    }
}
