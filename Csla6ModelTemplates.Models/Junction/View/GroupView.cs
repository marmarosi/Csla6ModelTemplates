using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Junction.View;
using Csla6ModelTemplates.CslaExtensions.Models;

namespace Csla6ModelTemplates.Models.Junction.View
{
    /// <summary>
    /// Represents a read-only group object.
    /// </summary>
    [Serializable]
    public class GroupView : ReadOnlyModel<GroupView>
    {
        #region Properties

        public static readonly PropertyInfo<long?> GroupKeyProperty = RegisterProperty<long?>(nameof(GroupKey));
        public long? GroupKey
        {
            get => GetProperty(GroupKeyProperty);
            private set => LoadProperty(GroupKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> GroupIdProperty = RegisterProperty<long?>(nameof(GroupId), RelationshipTypes.PrivateField);
        public string GroupId
        {
            get => KeyHash.Encode(ID.Group, GroupKey);
            private set => GroupKey = KeyHash.Decode(ID.Group, value);
        }

        public static readonly PropertyInfo<string> GroupCodeProperty = RegisterProperty<string>(nameof(GroupCode));
        public string GroupCode
        {
            get => GetProperty(GroupCodeProperty);
            private set => LoadProperty(GroupCodeProperty, value);
        }

        public static readonly PropertyInfo<string> GroupNameProperty = RegisterProperty<string>(nameof(GroupName));
        public string GroupName
        {
            get => GetProperty(GroupNameProperty);
            private set => LoadProperty(GroupNameProperty, value);
        }

        public static readonly PropertyInfo<GroupPersonViews> PersonsProperty = RegisterProperty<GroupPersonViews>(nameof(Persons));
        public GroupPersonViews Persons
        {
            get => GetProperty(PersonsProperty);
            private set => LoadProperty(PersonsProperty, value);
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        new IsInRole(
        //            AuthorizationActions.ReadProperty,
        //            GroupNameProperty,
        //            "Manager"
        //            )
        //        );
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(GroupView),
        //        new IsInRole(
        //            AuthorizationActions.GetObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets the specified read-only group instance.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="id">The identifier of the team.</param>
        /// <returns>The requested read-only group instance.</returns>
        public static async Task<GroupView> Get(
            IDataPortalFactory factory,
            string id
            )
        {
            var criteria = new GroupViewParams(id);
            return await factory.GetPortal<GroupView>().FetchAsync(criteria.Decode());
        }

        #endregion

        #region Data Access

        [Fetch]
        private void Fetch(
            GroupViewCriteria criteria,
            [Inject] IGroupViewDal dal,
            [Inject] IChildDataPortal<GroupPersonViews> itemsPortal
            )
        {
            // Load values from persistent storage.
            GroupViewDao dao = dal.Fetch(criteria);
            DataMapper.Map(dao, this, "Persons");
            Persons = itemsPortal.FetchChild(dao.Persons);
        }

        #endregion
    }
}
