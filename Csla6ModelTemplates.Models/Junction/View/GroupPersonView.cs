using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Junction.View;
using Csla6ModelTemplates.CslaExtensions.Models;
using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Models.Junction.View
{
    /// <summary>
    /// Represents an item in a read-only group-person collection.
    /// </summary>
    [Serializable]
    public class GroupPersonView : ReadOnlyModel<GroupPersonView>
    {
        #region Properties

        public static readonly PropertyInfo<long?> PersonKeyProperty = RegisterProperty<long?>(nameof(PersonKey));
        public long? PersonKey
        {
            get => GetProperty(PersonKeyProperty);
            private set => LoadProperty(PersonKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> PersonIdProperty = RegisterProperty<long?>(nameof(PersonId), RelationshipTypes.PrivateField);
        public string PersonId
        {
            get => KeyHash.Encode(ID.Person, PersonKey);
            private set => PersonKey = KeyHash.Decode(ID.Person, value);
        }

        public static readonly PropertyInfo<string> PersonNameProperty = RegisterProperty<string>(nameof(PersonName));
        public string PersonName
        {
            get => GetProperty(PersonNameProperty);
            private set => LoadProperty(PersonNameProperty, value);
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        new IsInRole(
        //            AuthorizationActions.ReadProperty,
        //            PersonNameProperty,
        //            "Manager"
        //            )
        //        );
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(GroupPersonView),
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
            GroupPersonViewDao dao
            )
        {
            // Load values from persistent storage.
            DataMapper.Map(dao, this);
        }

        #endregion
    }
}
