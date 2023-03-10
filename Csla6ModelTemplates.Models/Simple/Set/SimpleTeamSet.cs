using Csla;
using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.CslaExtensions.Models;
using Csla6ModelTemplates.Dal;
using Microsoft.AspNetCore.Mvc;

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

        #region Business Methods

        /// <summary>
        /// Rebuilds an editable team collection from the data transfer objects.
        /// </summary>
        /// <param name="list">The list of data transfer objects.</param>
        internal async Task Update(
            List<SimpleTeamSetItemDto> list,
            IDataPortal<SimpleTeamSetItem> portal
            )
        {
            await Update(list, "TeamId", portal);
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Rebuilds an editable team instance from the data transfer object.
        /// </summary>
        /// <param name="criteria">The criteria of the editable team collection.</param>
        /// <param name="dto">The data transfer object.</param>
        /// <returns>The rebuilt editable team instance.</returns>
        public static async Task<SimpleTeamSet> FromDto(
            SimpleTeamSetCriteria criteria,
            List<SimpleTeamSetItemDto> list,
            IDataPortal<SimpleTeamSet> portal,
            IDataPortal<SimpleTeamSetItem> itemPortal
            )
        {
            SimpleTeamSet set = await portal.FetchAsync(criteria);
            await set.Update(list, itemPortal);
            return set;
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            SimpleTeamSetCriteria criteria,
            [Inject] ISimpleTeamSetDal dal,
            [Inject] IChildDataPortal<SimpleTeamSetItem> childPortal
            )
        {
            // Load values from persistent storage.
            using (LoadListMode)
            {
                List<SimpleTeamSetItemDao> list = dal.Fetch(criteria);
                foreach (SimpleTeamSetItemDao item in list)
                    Add(childPortal.FetchChild(item));
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
