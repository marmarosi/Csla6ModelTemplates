using Csla;
using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Simple.Set
{
    /// <summary>
    /// Represents an editable team collection.
    /// </summary>
    [Serializable]
    public class SimpleTeamSet : EditableList<SimpleTeamSet, SimpleTeamSetItem, SimpleTeamSetItemDto>
    {
        #region Business Rules

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(SimpleTeamSet),
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
        /// <param name="dto">The data transfer object.</param>
        /// <param name="portal">The data portal of the collection.</param>
        /// <param name="itemPortal">The data portal of items.</param>
        /// <returns>The rebuilt editable team instance.</returns>
        public static async Task<SimpleTeamSet> FromDto(
            SimpleTeamSetCriteria criteria,
            List<SimpleTeamSetItemDto> dto,
            IDataPortal<SimpleTeamSet> portal,
            IChildDataPortal<SimpleTeamSetItem> itemPortal
            )
        {
            SimpleTeamSet set = await portal.FetchAsync(criteria);
            set.UpdateById(dto, "TeamId", itemPortal);
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
            SimpleTeamSetCriteria criteria,
            [Inject] ISimpleTeamSetDal dal,
            [Inject] IChildDataPortal<SimpleTeamSetItem> itemPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<SimpleTeamSetItemDao> list = dal.Fetch(criteria);
                foreach (SimpleTeamSetItemDao item in list)
                    Add(itemPortal.FetchChild(item));
            }
        }

        [Update]
        protected void Update(
            [Inject] ISimpleTeamSetDal dal
            )
        {
            // Update values in persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                base.Child_Update();
                dal.Commit(transaction);
            }
        }

        #endregion
    }
}
