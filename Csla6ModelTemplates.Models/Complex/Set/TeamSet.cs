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
        /// Rebuilds an editable team instance from the data transfer object.
        /// </summary>
        /// <param name="criteria">The criteria of the editable team collection.</param>
        /// <param name="list">The data transfer object.</param>
        /// <param name="portal">The data portal of the collection.</param>
        /// <param name="itemPortal">The data portal of items.</param>
        /// <returns>The rebuilt editable team instance.</returns>
        public static async Task<TeamSet> FromDto(
            TeamSetCriteria criteria,
            List<TeamSetItemDto> list,
            [Inject] IDataPortal<TeamSet> portal,
            [Inject] IChildDataPortal<TeamSetItem> itemPortal
            )
        {
            TeamSet set = await portal.FetchAsync(criteria);
            set.UpdateById(list, "TeamId", itemPortal);
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
