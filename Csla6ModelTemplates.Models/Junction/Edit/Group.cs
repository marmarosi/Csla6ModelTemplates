using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Models.Junction.Edit
{
    /// <summary>
    /// Represents an editable group object.
    /// </summary>
    [Serializable]
    [ValidationResourceType(typeof(ValidationText), ObjectName = "Group")]
    public class Group : EditableModel<Group, GroupDto>
    {
        #region Properties

        public static readonly PropertyInfo<long?> GroupKeyProperty = RegisterProperty<long?>(nameof(GroupKey));
        public long? GroupKey
        {
            get => GetProperty(GroupKeyProperty);
            private set => SetProperty(GroupKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> GroupIdProperty = RegisterProperty<long?>(nameof(GroupId), RelationshipTypes.PrivateField);
        public string GroupId
        {
            get => KeyHash.Encode(ID.Group, GroupKey);
            set => GroupKey = KeyHash.Decode(ID.Group, value);
        }

        public static readonly PropertyInfo<string> GroupCodeProperty = RegisterProperty<string>(nameof(GroupCode));
        [Required]
        [MaxLength(10)]
        public string GroupCode
        {
            get => GetProperty(GroupCodeProperty);
            set => SetProperty(GroupCodeProperty, value);
        }

        public static readonly PropertyInfo<string> GroupNameProperty = RegisterProperty<string>(nameof(GroupName));
        [Required]
        [MaxLength(100)]
        public string GroupName
        {
            get => GetProperty(GroupNameProperty);
            set => SetProperty(GroupNameProperty, value);
        }

        public static readonly PropertyInfo<GroupPersons> GroupPersonsProperty = RegisterProperty<GroupPersons>(nameof(Persons));
        public GroupPersons Persons
        {
            get => GetProperty(GroupPersonsProperty);
            private set => LoadProperty(GroupPersonsProperty, value);
        }

        public static readonly PropertyInfo<DateTimeOffset?> TimestampProperty = RegisterProperty<DateTimeOffset?>(nameof(Timestamp));
        public DateTimeOffset? Timestamp
        {
            get => GetProperty(TimestampProperty);
            private set => LoadProperty(TimestampProperty, value);
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Call base class implementation to add data annotation rules to BusinessRules.
        //    // NOTE: DataAnnotation rules is always added with Priority = 0.
        //    base.AddBusinessRules();

        //    // Add validation rules.
        //    BusinessRules.AddRule(new Required(GroupNameProperty));

        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        new IsInRole(
        //            AuthorizationActions.WriteProperty,
        //            GroupNameProperty,
        //            "Manager"
        //            )
        //        );
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(Group),
        //        new IsInRole(
        //            AuthorizationActions.EditObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Business Methods

        /// <summary>
        /// Updates an editable model and its children from the data transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        public override void SetValuesOnBuild(
            GroupDto dto,
            IChildDataPortalFactory childFactory
            )
        {
            DataMapper.Map(dto, this, "Persons");
            BusinessRules.CheckRules();
            Persons.SetValuesById(dto.Persons, "PersonId", childFactory);
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets a new group to edit.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <returns>The new group.</returns>
        public static async Task<Group> New(
            IDataPortalFactory factory
            )
        {
            return await factory.GetPortal<Group>().CreateAsync();
        }

        /// <summary>
        /// Gets an existing editable group instance.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="id">The identifier of the group.</param>
        /// <returns>The requested editable group instance.</returns>
        public static async Task<Group> Get(
            IDataPortalFactory factory,
            string id
            )
        {
            var criteria = new GroupParams(id);
            return await factory.GetPortal<Group>().FetchAsync(criteria.Decode());
        }

        /// <summary>
        /// Builds a new or existing team.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        /// <param name="dto"></param>
        /// <returns>The team built.</returns>
        public static async Task<Group> Build(
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory,
            GroupDto dto
            )
        {
            long? groupKey = KeyHash.Decode(ID.Group, dto.GroupId);
            Group team = groupKey.HasValue ?
                await Get(factory, dto.GroupId) :
                await New(factory);
            team.SetValuesOnBuild(dto, childFactory);
            return team;
        }

        /// <summary>
        /// Deletes an existing group.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="id">The identifier of the group.</param>
        public static async Task Delete(
            IDataPortalFactory factory,
            string id
            )
        {
            var criteria = new GroupParams(id);
            await factory.GetPortal<Group>().DeleteAsync(criteria.Decode());
        }

        #endregion

        #region Data Access

        [Create]
        [RunLocal]
        private void Create(
            [Inject] IChildDataPortal<GroupPersons> itemsPortal
            )
        {
            // Load default values.
            Persons = itemsPortal.CreateChild();
            //LoadProperty(GroupCodeProperty, "");
            //BusinessRules.CheckRules();
        }

        [Fetch]
        private void Fetch(
            GroupCriteria criteria,
            [Inject] IGroupDal dal,
            [Inject] IChildDataPortal<GroupPersons> itemsPortal
            )
        {
            // Load values from persistent storage.
            GroupDao dao = dal.Fetch(criteria);
            using (BypassPropertyChecks)
            {
                DataMapper.Map(dao, this, "Persons");
                Persons = itemsPortal.FetchChild(dao.Persons);
            }
        }

        [Insert]
        protected void Insert(
            [Inject] IGroupDal dal
            )
        {
            // Insert values into persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                using (BypassPropertyChecks)
                {
                    var dao = Copy.PropertiesFrom(this).Omit("Persons").ToNew<GroupDao>();
                    dal.Insert(dao);

                    // Set new data.
                    GroupKey = dao.GroupKey;
                    Timestamp = dao.Timestamp;
                }
                FieldManager.UpdateChildren(this);
                dal.Commit(transaction);
            }
        }

        [Update]
        protected void Update(
            [Inject] IGroupDal dal
            )
        {
            // Update values in persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                if (IsSelfDirty)
                {
                    using (BypassPropertyChecks)
                    {
                        var dao = Copy.PropertiesFrom(this).Omit("Persons").ToNew<GroupDao>();
                        dal.Update(dao);

                        // Set new data.
                        Timestamp = dao.Timestamp;
                    }
                }
                FieldManager.UpdateChildren(this);
                dal.Commit(transaction);
            }
        }

        [DeleteSelf]
        protected void DeleteSelf(
            [Inject] IGroupDal dal,
            [Inject] IChildDataPortal<GroupPersons> itemPortal
            )
        {
            using (BypassPropertyChecks)
                Delete(new GroupCriteria(GroupKey), dal, itemPortal);
        }

        [Delete]
        protected void Delete(
            GroupCriteria criteria,
            [Inject] IGroupDal dal,
            [Inject] IChildDataPortal<GroupPersons> itemPortal
            )
        {
            // Delete values from persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                if (!GroupKey.HasValue)
                    Fetch(criteria, dal, itemPortal);

                Persons.Clear();
                FieldManager.UpdateChildren(this);

                dal.Delete(criteria);
                dal.Commit(transaction);
            }
        }

        #endregion
    }
}
