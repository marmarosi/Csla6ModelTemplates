using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Arrangement.Sorting;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Arrangement.Sorting
{
    /// <summary>
    /// Represents an item in a read-only team collection.
    /// </summary>
    [Serializable]
    public class SortedTeamListItem : ReadOnlyModel<SortedTeamListItem>
    {
        #region Properties

        public static readonly PropertyInfo<long?> TeamKeyProperty = RegisterProperty<long?>(nameof(TeamKey));
        public long? TeamKey
        {
            get => GetProperty(TeamKeyProperty);
            private set => LoadProperty(TeamKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> TeamIdProperty = RegisterProperty<long?>(nameof(TeamId), RelationshipTypes.PrivateField);
        public string TeamId
        {
            get => KeyHash.Encode(ID.Team, TeamKey);
            private set => TeamKey = KeyHash.Decode(ID.Team, value);
        }

        public static readonly PropertyInfo<string> TeamCodeProperty = RegisterProperty<string>(nameof(TeamCode));
        public string TeamCode
        {
            get => GetProperty(TeamCodeProperty);
            private set => LoadProperty(TeamCodeProperty, value);
        }

        public static readonly PropertyInfo<string> TeamNameProperty = RegisterProperty<string>(nameof(TeamName));
        public string TeamName
        {
            get => GetProperty(TeamNameProperty);
            private set => LoadProperty(TeamNameProperty, value);
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        new IsInRole(
        //            AuthorizationActions.ReadProperty,
        //            TeamNameProperty,
        //            "Manager"
        //            )
        //        );
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(SortedTeamListItem),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Data Access

        [FetchChild]
        protected void Fetch(
            SortedTeamListItemDao dao
            )
        {
            // Set values from data access object.
            DataMapper.Map(dao, this);
        }

        #endregion
    }
}
