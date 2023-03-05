using Csla;
using Csla.Core;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.CslaExtensions.Models;
using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Models.Simple.Set
{
    /// <summary>
    /// Represents an editable child object.
    /// </summary>
    [Serializable]
    [ValidationResourceType(typeof(ValidationText), ObjectName = "SimpleTeamSetItem")]
    public class SimpleTeamSetItem : EditableModel<SimpleTeamSetItem, SimpleTeamSetItemDto>
    {
        #region Properties

        public static readonly PropertyInfo<long?> TeamKeyProperty = RegisterProperty<long?>(nameof(TeamKey));
        public long? TeamKey
        {
            get => GetProperty(TeamKeyProperty);
            private set => SetProperty(TeamKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> TeamIdProperty = RegisterProperty<long?>(nameof(TeamId), RelationshipTypes.PrivateField);
        public string TeamId
        {
            get => KeyHash.Encode(ID.Team, TeamKey);
            set => TeamKey = KeyHash.Decode(ID.Team, value);
        }

        public static readonly PropertyInfo<string> TeamCodeProperty = RegisterProperty<string>(nameof(TeamCode));
        [Required]
        [MaxLength(10)]
        public string TeamCode
        {
            get { return GetProperty(TeamCodeProperty); }
            set { SetProperty(TeamCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> TeamNameProperty = RegisterProperty<string>(nameof(TeamName));
        [Required]
        [MaxLength(100)]
        public string TeamName
        {
            get { return GetProperty(TeamNameProperty); }
            set { SetProperty(TeamNameProperty, value); }
        }

        public static readonly PropertyInfo<DateTime?> TimestampProperty = RegisterProperty<DateTime?>(nameof(Timestamp));
        public DateTime? Timestamp
        {
            get { return GetProperty(TimestampProperty); }
            private set { LoadProperty(TimestampProperty, value); }
        }

        #endregion

        #region Business Rules

        //protected override void AddBusinessRules()
        //{
        //    // Call base class implementation to add data annotation rules to BusinessRules.
        //    // NOTE: DataAnnotation rules is always added with Priority = 0.
        //    base.AddBusinessRules();

        //    // Add validation rules.
        //    BusinessRules.AddRule(new Required(TeamNameProperty));

        //    // Add authorization rules.
        //    BusinessRules.AddRule(new IsInRole(
        //        AuthorizationActions.WriteProperty,
        //        TeamNameProperty,
        //        "Manager"
        //        ));
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(SimpleTeamSetItem),
        //        new IsInRole(
        //            AuthorizationActions.EditObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Business Methods

        /// <summary>
        /// Updates an editable team from the data transfer object.
        /// </summary>
        /// <param name="dto">The data transfer objects.</param>
        public override async Task Update(
            SimpleTeamSetItemDto dto
            )
        {
            //TeamKey = KeyHash.Decode(ID.Team, dto.TeamId);
            TeamCode = dto.TeamCode;
            TeamName = dto.TeamName;
            //Timestamp = dto.Timestamp;

            await base.Update(dto);
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Creates an editable team instance from the data transfer object.
        /// </summary>
        /// <param name="parent">The parent collection.</param>
        /// <param name="dto">The data transfer object.</param>
        /// <returns>The new editable team instance.</returns>
        internal static new async Task<SimpleTeamSetItem> Create(
            IParent parent,
            SimpleTeamSetItemDto dto
            )
        {
            return await Create(parent, dto);
        }

        #endregion

        #region Data Access

        //[CreateChild]
        //private void Create()
        //{
        //    // Load default values.
        //    // Omit this override if you have no defaults to set.
        //    LoadProperty(TeamCodeProperty, "");
        //    BusinessRules.CheckRules();
        //}

        [FetchChild]
        private void Fetch(
            SimpleTeamSetItemDao dao
            )
        {
            using (BypassPropertyChecks)
            {
                // Set values from data access object.
                TeamKey = dao.TeamKey;
                TeamCode = dao.TeamCode;
                TeamName = dao.TeamName;
                Timestamp = dao.Timestamp;
            }
        }

        private SimpleTeamSetItemDao CreateDao()
        {
            // Build the data access object.
            return new SimpleTeamSetItemDao
            {
                TeamKey = TeamKey,
                TeamCode = TeamCode,
                TeamName = TeamName,
                Timestamp = Timestamp
            };
        }

        [InsertChild]
        private void Insert(
            [Inject] ISimpleTeamSetItemDal dal
            )
        {
            // Insert values into persistent storage.
            using (BypassPropertyChecks)
            {
                SimpleTeamSetItemDao dao = CreateDao();
                dal.Insert(dao);

                // Set new data.
                TeamKey = dao.TeamKey;
                Timestamp = dao.Timestamp;
            }
        }

        [UpdateChild]
        private void Update(
            [Inject] ISimpleTeamSetItemDal dal
            )
        {
            // Update values in persistent storage.
            using (BypassPropertyChecks)
            {
                SimpleTeamSetItemDao dao = CreateDao();
                dal.Update(dao);

                // Set new data.
                Timestamp = dao.Timestamp;
            }
        }

        [DeleteSelfChild]
        private void DeleteSelf(
            [Inject] ISimpleTeamSetItemDal dal
            )
        {
            // Delete values from persistent storage.
            if (TeamKey.HasValue)
            {
                SimpleTeamSetItemCriteria criteria = new SimpleTeamSetItemCriteria(TeamKey.Value);
                dal.Delete(criteria);
            }
        }

        #endregion
    }
}
