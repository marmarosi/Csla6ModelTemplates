using Csla;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Complex.Set
{
    /// <summary>
    /// Represents an editable team collection.
    /// </summary>
    [Serializable]
    public class TeamSet : EditableList<TeamSet, TeamSetItem, TeamSetItemDto>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(TeamSet),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets the specified team set to edit.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="criteria">The criteria of the team set.</param>
        /// <returns>The requested team set.</returns>
        public static async Task<TeamSet> Get(
            IDataPortalFactory factory,
            TeamSetCriteria criteria
            )
        {
            return await factory.GetPortal<TeamSet>().FetchAsync(criteria);
        }

        /// <summary>
        /// Rebuilds an editable team instance from the data transfer object.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        /// <param name="criteria">The criteria of the team set.</param>
        /// <param name="list">The data transer objects of the team set.</param>
        /// <returns>The team set built.</returns>
        public static async Task<TeamSet> Build(
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory,
            TeamSetCriteria criteria,
            List<TeamSetItemDto> list
            )
        {
            var set = await factory.GetPortal<TeamSet>().FetchAsync(criteria);
            set.SetValuesById(list, "TeamId", childFactory);
            return set;
        }

        #endregion

        #region Data Access

        [Create]
        [RunLocal]
        private void Create()
        {
            // Load default values.
        }

        [Fetch]
        private void Fetch(
            TeamSetCriteria criteria,
            [Inject] ITeamSetDal dal,
            [Inject] IChildDataPortal<TeamSetItem> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<TeamSetItemDao> list = dal.Fetch(criteria);
                foreach (TeamSetItemDao item in list)
                    Add(itemPortal.FetchChild(item));
            }
        }

        [Update]
        protected void Update(
            [Inject] ITeamSetDal dal
            )
        {
            // Update values in persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                Child_Update();
                dal.Commit(transaction);
            }
        }

        #endregion
    }
}
