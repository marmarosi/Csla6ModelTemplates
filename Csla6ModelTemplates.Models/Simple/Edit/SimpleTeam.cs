using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Models.Simple.Edit
{
    /// <summary>
    /// Represents an editable team object.
    /// </summary>
    [Serializable]
    [ValidationResourceType(typeof(ValidationText), ObjectName = "SimpleTeam")]
    public class SimpleTeam : EditableModel<SimpleTeam, SimpleTeamDto>
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
            get => GetProperty(TeamCodeProperty);
            set => SetProperty(TeamCodeProperty, value);
        }

        public static readonly PropertyInfo<string> TeamNameProperty = RegisterProperty<string>(nameof(TeamName));
        [Required]
        [MaxLength(100)]
        public string TeamName
        {
            get => GetProperty(TeamNameProperty);
            set => SetProperty(TeamNameProperty, value);
        }

        public static readonly PropertyInfo<DateTimeOffset?> TimestampProperty = RegisterProperty<DateTimeOffset?>(nameof(Timestamp));
        public DateTimeOffset? Timestamp
        {
            get =>  GetProperty(TimestampProperty);
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
        //    BusinessRules.AddRule(new Required(TeamNameProperty));

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
        //        typeof(SimpleTeam),
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
            SimpleTeamDto dto,
            IChildDataPortalFactory childFactory
            )
        {
            DataMapper.Map(dto, this);
            BusinessRules.CheckRules();
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets a new team to edit.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <returns>The new team.</returns>
        public static async Task<SimpleTeam> New(
            IDataPortalFactory factory
            )
        {
            return await factory.GetPortal<SimpleTeam>().CreateAsync();
        }

        /// <summary>
        /// Gets the specified team to edit.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="id">The identifier of the team.</param>
        /// <returns>The requested team.</returns>
        public static async Task<SimpleTeam> Get(
            IDataPortalFactory factory,
            string id
            )
        {
            var criteria = new SimpleTeamParams(id);
            return await factory.GetPortal<SimpleTeam>().FetchAsync(criteria.Decode());
        }

        /// <summary>
        /// Builds a new or existing team.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        /// <param name="dto"></param>
        /// <returns>The team built.</returns>
        public static async Task<SimpleTeam> Build(
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory,
            SimpleTeamDto dto
            )
        {
            long? teamKey = KeyHash.Decode(ID.Team, dto.TeamId);
            SimpleTeam team = teamKey.HasValue ?
                await Get(factory, dto.TeamId) :
                await New(factory);
            team.SetValuesOnBuild(dto, childFactory);
            return team;
        }

        /// <summary>
        /// Deletes the specified team.
        /// </summary>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="id">The identifier of the team.</param>
        public static async Task Delete(
            IDataPortalFactory factory,
            string id
            )
        {
            var criteria = new SimpleTeamParams(id);
            await factory.GetPortal<SimpleTeam>().DeleteAsync(criteria.Decode());
        }

        #endregion

        #region Data Access

        [Create]
        [RunLocal]
        private void Create()
        {
            // Load default values.
            //LoadProperty(TeamCodeProperty, "");
            //BusinessRules.CheckRules();
        }

        [Fetch]
        private void Fetch(
            SimpleTeamCriteria criteria,
            [Inject] ISimpleTeamDal dal
            )
        {
            // Load values from persistent storage.
            SimpleTeamDao dao = dal.Fetch(criteria);
            using (BypassPropertyChecks)
                DataMapper.Map(dao, this);
        }

        [Insert]
        protected void Insert(
            [Inject] ISimpleTeamDal dal
            )
        {
            // Insert values into persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                using (BypassPropertyChecks)
                {
                    var dao = Copy.PropertiesFrom(this).ToNew<SimpleTeamDao>();
                    dal.Insert(dao);

                    // Set new data.
                    TeamKey = dao.TeamKey;
                    Timestamp = dao.Timestamp;
                }
                dal.Commit(transaction);
            }
        }

        [Update]
        protected void Update(
            [Inject] ISimpleTeamDal dal
            )
        {
            // Update values in persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                using (BypassPropertyChecks)
                {
                    var dao = Copy.PropertiesFrom(this).ToNew<SimpleTeamDao>();
                    dal.Update(dao);

                    // Set new data.
                    Timestamp = dao.Timestamp;
                }
                dal.Commit(transaction);
            }
        }

        [DeleteSelf]
        protected void DeleteSelf(
            [Inject] ISimpleTeamDal dal
            )
        {
            using (BypassPropertyChecks)
                Delete(new SimpleTeamCriteria(TeamKey), dal);
        }

        [Delete]
        protected void Delete(
            SimpleTeamCriteria criteria,
            [Inject] ISimpleTeamDal dal
            )
        {
            // Delete values from persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                dal.Delete(criteria);
                dal.Commit(transaction);
            }
        }

        #endregion
    }
}
