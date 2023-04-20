using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.CslaExtensions;

namespace Csla6ModelTemplates.Models.Complex.Command
{
    /// <summary>
    /// Represents an item in a read-only count result collection.
    /// </summary>
    [Serializable]
    public class CountTeamsResult : ReadOnlyModel<CountTeamsResult>
    {
        #region Properties

        public static readonly PropertyInfo<int> ItemCountProperty = RegisterProperty<int>(nameof(ItemCount));
        public int ItemCount
        {
            get => GetProperty(ItemCountProperty);
            private set => LoadProperty(ItemCountProperty, value);
        }

        public static readonly PropertyInfo<int> CountOfTeamsProperty = RegisterProperty<int>(nameof(CountOfTeams));
        public int CountOfTeams
        {
            get => GetProperty(CountOfTeamsProperty);
            private set => LoadProperty(CountOfTeamsProperty, value);
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        new IsInRole(
        //            AuthorizationActions.WriteProperty,
        //            ItemCountProperty,
        //            "Manager"
        //            )
        //        );
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(CountTeamsListItem),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Data Access

        [FetchChild]
        private void Fetch(
            CountTeamsResultDao dao
            )
        {
            // Load values from persistent storage.
            DataMapper.Map(dao, this);
        }

        #endregion
    }
}
