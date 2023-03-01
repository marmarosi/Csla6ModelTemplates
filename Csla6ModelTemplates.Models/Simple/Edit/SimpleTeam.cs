using Csla;
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

        private long? TeamKey
        {
            get { return KeyHash.Decode(ID.Team, TeamId); }
            set { TeamId = KeyHash.Encode(ID.Team, value); }
        }

        public static readonly PropertyInfo<string> TeamIdProperty = RegisterProperty<string>(c => c.TeamId);
        public string TeamId
        {
            get { return GetProperty(TeamIdProperty); }
            private set { SetProperty(TeamIdProperty, value); }
        }

        public static readonly PropertyInfo<string> TeamCodeProperty = RegisterProperty<string>(c => c.TeamCode);
        [Required]
        [MaxLength(10)]
        public string TeamCode
        {
            get { return GetProperty(TeamCodeProperty); }
            set { SetProperty(TeamCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> TeamNameProperty = RegisterProperty<string>(c => c.TeamName);
        [Required]
        [MaxLength(100)]
        public string TeamName
        {
            get { return GetProperty(TeamNameProperty); }
            set { SetProperty(TeamNameProperty, value); }
        }

        public static readonly PropertyInfo<DateTime?> TimestampProperty = RegisterProperty<DateTime?>(c => c.Timestamp);
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

        /// <summary>
        /// Updates an editable team from the data transfer object.
        /// </summary>
        /// <param name="data">The data transfer object.</param>
        public override async Task Update(
                SimpleTeamDto dto
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
            await team.Update(dto);
            return team;
        }

        #endregion

        #region Data Access

        [Create]
        [RunLocal]
        private void Create()
        {
            // Load default values.
            // Omit this override if you have no defaults to set.
            LoadProperty(TeamCodeProperty, "");
            BusinessRules.CheckRules();
        }

        [Fetch]
        private void Fetch(
            SimpleTeamCriteria criteria,
            [Inject] ISimpleTeamDal dal
            )
        {
            // Load values from persistent storage.
            SimpleTeamDao dao = dal.Get(criteria);
            using (BypassPropertyChecks)
            {
                TeamKey = dao.TeamKey;
                TeamCode = dao.TeamCode;
                TeamName = dao.TeamName;
                Timestamp = dao.Timestamp;
            }
        }

        private SimpleTeamDao CreateDao()
        {
            // Build the data access object.
            return new SimpleTeamDao
            {
                TeamKey = TeamKey,
                TeamCode = TeamCode,
                TeamName = TeamName,
                Timestamp = Timestamp
            };
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
                SimpleTeamDao dao = CreateDao();
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
                SimpleTeamDao dao = CreateDao();
                dal.Update(dao);

                // Set new data.
                Timestamp = dao.Timestamp;
            }
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        [DeleteSelf]
        protected void DeleteSelf(
            [Inject] ISimpleTeamDal dal
            )
        {
            if (TeamKey.HasValue)
                using (BypassPropertyChecks)
                    Delete(new SimpleTeamCriteria(TeamKey.Value), dal);
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
