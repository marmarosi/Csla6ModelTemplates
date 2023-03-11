using Csla;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.CslaExtensions.Models
{
    /// <summary>
    /// Represents a key-name option in a read-only choice object.
    /// </summary>
    [Serializable]
    public class IdNameOption : ReadOnlyModel<IdNameOption>
    {
        #region Business Methods

        private readonly string _hashid;

        public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(c => c.Id, RelationshipTypes.PrivateField);
        private long? Key = null;
        public string Id
        {
            get { return GetProperty(IdProperty, KeyHash.Encode(_hashid, Key)); }
            private set { Key = KeyHash.Decode(_hashid, value); }
        }

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
        public string Name
        {
            get { return GetProperty(NameProperty); }
            private set { LoadProperty(NameProperty, value); }
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

        private void Child_Fetch(
            IdNameOptionDao dao
            )
        {
            Key = dao.Key;
            Name = dao.Name;
        }

        #endregion
    }
}
