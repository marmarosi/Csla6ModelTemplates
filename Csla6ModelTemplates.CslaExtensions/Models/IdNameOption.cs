using Csla;
using Csla.Data;
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

        private string HashModel;

        public static readonly PropertyInfo<long?> KeyProperty = RegisterProperty<long?>(nameof(Key));
        public long? Key
        {
            get => GetProperty(KeyProperty);
            private set => LoadProperty(KeyProperty, value);
        }

        public static readonly PropertyInfo<long?> IdProperty = RegisterProperty<long?>(nameof(Id), RelationshipTypes.PrivateField);
        public string Id
        {
            get => KeyHash.Encode(HashModel, Key);
            private set => Key = KeyHash.Decode(HashModel, value);
        }

        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get => GetProperty(NameProperty);
            private set => LoadProperty(NameProperty, value);
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
        //        typeof(IdNameOption),
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
            IdNameOptionDao dao,
            string hashModel
            )
        {
            // Set values from data access object.
            HashModel = hashModel;
            DataMapper.Map(dao, this);
        }

        #endregion
    }
}
