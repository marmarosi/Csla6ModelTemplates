using Csla;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.CslaExtensions.Models
{
    /// <summary>
    /// Represents a key-name option in a read-only choice object.
    /// </summary>
    [Serializable]
    public class KeyNameOption : ReadOnlyModel<KeyNameOption>
    {
        #region Business Methods

        public static readonly PropertyInfo<long?> KeyProperty = RegisterProperty<long?>(c => c.Key);
        public long? Key
        {
            get { return GetProperty(KeyProperty); }
            private set { LoadProperty(KeyProperty, value); }
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

        [FetchChild]
        private void Fetch(
            KeyNameOptionDao dao
            )
        {
            Key = dao.Key;
            Name = dao.Name;
        }

        #endregion
    }
}
