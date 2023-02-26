using Csla;
using Csla.Rules;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Simple.View
{
    /// <summary>
    /// Represents a read-only team object.
    /// </summary>
    [Serializable]
    public class SimpleTeamView : ReadOnlyModel<SimpleTeamView>
    {
        #region Properties

        public static readonly PropertyInfo<long?> TeamKeyProperty = RegisterProperty<long?>(nameof(TeamKey));
        public long? TeamKey
        {
            get => GetProperty(TeamKeyProperty);
            private set => LoadProperty(TeamKeyProperty, value);
        }

        public static readonly PropertyInfo<string> TeamIdProperty = RegisterProperty<string>(nameof(TeamId), RelationshipTypes.PrivateField);
        public string TeamId
        {
            get => GetProperty(TeamIdProperty, KeyHash.Encode(ID.Team, TeamKey));
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
        //    BusinessRules.AddRule(new IsInRole(
        //        AuthorizationActions.ReadProperty,
        //        TeamNameProperty,
        //        "Manager"
        //        ));
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(SimpleTeamView),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            SimpleTeamViewCriteria criteria,
            [Inject] ISimpleTeamViewDal dal
            )
        {
            var dao = dal.GetView(criteria);
            Csla.Data.DataMapper.Map(dao, this);
            BusinessRules.CheckRules();
        }

        #endregion
    }
}
