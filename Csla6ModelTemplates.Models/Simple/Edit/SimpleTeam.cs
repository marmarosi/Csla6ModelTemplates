using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.CslaExtensions.Models;
using Csla6ModelTemplates.CslaExtensions.Validations;
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
        //        typeof(SimpleTeam),
        //        new IsInRole(
        //            AuthorizationActions.EditObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Business Methods

        ///// <summary>
        ///// Updates an editable team from the data transfer object.
        ///// </summary>
        ///// <param name="data">The data transfer object.</param>
        //public override async Task Update(
        //        SimpleTeamDto dto
        //        )
        //{
        //    Copy.
        //    var copier = new PropertyCopier.Copier();
        //    copier.IgnoreProperty<SimpleTeamDto, this > (t => t.TeamKey);
        //    var dto = copier.From(SimpleTeamDto).To<this> ();

        //    //TeamKey = KeyHash.Decode(ID.Team, dto.TeamId);
        //    TeamCode = dto.TeamCode;
        //    TeamName = dto.TeamName;
        //    //Timestamp = dto.Timestamp;

        //    await base.Update(dto);
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Rebuilds an editable team instance from the data transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object.</param>
        /// <returns>The rebuilt editable team instance.</returns>
        public static async Task<SimpleTeam> FromDto(
            SimpleTeamDto dto,
            IDataPortal<SimpleTeam> portal
            )
        {
            long? teamKey = KeyHash.Decode(ID.Team, dto.TeamId);
            SimpleTeam team = teamKey.HasValue ?
                await portal.FetchAsync(new SimpleTeamCriteria(teamKey.Value)) :
                await portal.CreateAsync();

            Copy.PropertiesFrom(dto)
                .Omit("TeamKey", "Timestamp")
                .ToPropertiesOf(team);

            team.BusinessRules.CheckRules();
            return team;
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

        [Transactional(TransactionalTypes.TransactionScope)]
        [Insert]
        protected void Insert(
            [Inject] ISimpleTeamDal dal
            )
        {
            // Insert values into persistent storage.
            using (BypassPropertyChecks)
            {
                var dao = Copy.PropertiesFrom(this).ToNew<SimpleTeamDao>();
                dal.Insert(dao);

                // Set new data.
                TeamKey = dao.TeamKey;
                Timestamp = dao.Timestamp;
            }
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        [Update]
        protected void Update(
            [Inject] ISimpleTeamDal dal
            )
        {
            // Update values in persistent storage.
            using (BypassPropertyChecks)
            {
                var dao = Copy.PropertiesFrom(this).ToNew<SimpleTeamDao>();
                dal.Update(dao);

                // Set new data.
                Timestamp = dao.Timestamp;
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

        [Transactional(TransactionalTypes.TransactionScope)]
        [Delete]
        private void Delete(
            SimpleTeamCriteria criteria,
            [Inject] ISimpleTeamDal dal
            )
        {
            // Delete values from persistent storage.
            dal.Delete(criteria);
        }

        #endregion
    }
}
