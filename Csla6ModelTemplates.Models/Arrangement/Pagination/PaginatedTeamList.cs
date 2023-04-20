using Csla;
using Csla6ModelTemplates.Contracts.Arrangement.Pagination;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Arrangement.Pagination
{
    /// <summary>
    /// Represents a read-only paginated team collection.
    /// </summary>
    [Serializable]
    public class PaginatedTeamList : ReadOnlyModel<PaginatedTeamList>
    {
        #region Properties

        public static readonly PropertyInfo<PaginatedTeamListItems> PlayersProperty = RegisterProperty<PaginatedTeamListItems>(nameof(Data));
        public PaginatedTeamListItems Data
        {
            get => GetProperty(PlayersProperty);
            private set => LoadProperty(PlayersProperty, value);
        }

        public static readonly PropertyInfo<int> TotalCountProperty = RegisterProperty<int>(c => c.TotalCount);
        public int TotalCount
        {
            get => GetProperty(TotalCountProperty);
            private set => LoadProperty(TotalCountProperty, value);
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        new IsInRole(
        //            AuthorizationActions.ReadProperty,
        //            TotalCountProperty,
        //            "Manager"
        //            )
        //        );
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(PaginatedTeamList),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets the specified read-only paginated team collection.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="criteria">The criteria of the read-only team.</param>
        /// <returns>The requested read-only team instance.</returns>
        public static async Task<PaginatedTeamList> Get(
            IDataPortalFactory factory,
            PaginatedTeamListCriteria criteria
            )
        {
            return await factory.GetPortal<PaginatedTeamList>().FetchAsync(criteria);
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            PaginatedTeamListCriteria criteria,
            [Inject] IPaginatedTeamListDal dal,
            [Inject] IChildDataPortal<PaginatedTeamListItems> itemsPortal
            )
        {
            // Load values from persistent storage.
            IPaginatedList<PaginatedTeamListItemDao> dao = dal.Fetch(criteria);
            Data = itemsPortal.FetchChild(dao.Data);
            TotalCount = dao.TotalCount;
        }

        #endregion
    }
}
