using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.CslaExtensions.Models;
using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Models.Complex.Edit
{
    /// <summary>
    /// Represents an editable team object.
    /// </summary>
    [Serializable]
    [ValidationResourceType(typeof(ValidationText), ObjectName = "Team")]
    public class Team : EditableModel<Team, TeamDto>
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

        public static readonly PropertyInfo<Players> PlayersProperty = RegisterProperty<Players>(nameof(Players));
        public Players Players
        {
            get => GetProperty(PlayersProperty);
            private set => LoadProperty(PlayersProperty, value);
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
        //        typeof(Team),
        //        new IsInRole(
        //            AuthorizationActions.EditObject,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Factory Methods

        /// <summary>
        /// Rebuilds an editable team instance from the data transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object.</param>
        /// <param name="portal">The data portal of the model.</param>
        /// <param name="itemPortal">The data portal of the item model.</param>
        /// <returns>The rebuilt editable team instance.</returns>
        public static async Task<Team> FromDto(
            TeamDto dto,
            IDataPortal<Team> portal,
            IChildDataPortal<Player> itemPortal
            )
        {
            long? teamKey = KeyHash.Decode(ID.Team, dto.TeamId);
            Team team = teamKey.HasValue ?
                await portal.FetchAsync(new TeamCriteria(teamKey.Value)) :
                await portal.CreateAsync();

            DataMapper.Map(dto, team, "Players");
            team.Players.UpdateById(dto.Players, "PlayerId", itemPortal);
            return team;
        }

        #endregion

        #region Data Access

        [Create]
        [RunLocal]
        private void Create(
            [Inject] IChildDataPortal<Players> itemPortal
            )
        {
            // Load default values.
            //LoadProperty(TeamCodeProperty, "");
            //BusinessRules.CheckRules();
            Players = itemPortal.FetchChild(new List<PlayerDao>());
        }

        [Fetch]
        private void Fetch(
            TeamCriteria criteria,
            [Inject] ITeamDal dal,
            [Inject] IChildDataPortal<Players> itemPortal
            )
        {
            // Load values from persistent storage.
            TeamDao dao = dal.Fetch(criteria);
            using (BypassPropertyChecks)
            {
                DataMapper.Map(dao, this, "Players");
                Players = itemPortal.FetchChild(dao.Players);
            }
        }

        [Insert]
        protected void Insert(
            [Inject] ITeamDal dal
            )
        {
            // Insert values into persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                using (BypassPropertyChecks)
                {
                    var dao = Copy.PropertiesFrom(this).Omit("Players").ToNew<TeamDao>();
                    dal.Insert(dao);

                    // Set new data.
                    TeamKey = dao.TeamKey;
                    Timestamp = dao.Timestamp;
                }
                FieldManager.UpdateChildren(this);
                dal.Commit(transaction);
            }
        }

        [Update]
        protected void Update(
            [Inject] ITeamDal dal
            )
        {
            // Update values in persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                using (BypassPropertyChecks)
                {
                    var dao = Copy.PropertiesFrom(this).Omit("Players").ToNew<TeamDao>();
                    dal.Update(dao);

                    // Set new data.
                    Timestamp = dao.Timestamp;
                }
                FieldManager.UpdateChildren(this);
                dal.Commit(transaction);
            }
        }

        [DeleteSelf]
        protected void DeleteSelf(
            [Inject] ITeamDal dal,
            [Inject] IChildDataPortal<Players> itemPortal
            )
        {
            using (BypassPropertyChecks)
                Delete(new TeamCriteria(TeamKey), dal, itemPortal);
        }

        [Delete]
        protected void Delete(
            TeamCriteria criteria,
            [Inject] ITeamDal dal,
            [Inject] IChildDataPortal<Players> itemPortal
            )
        {
            // Delete values from persistent storage.
            using (var transaction = dal.BeginTransaction())
            {
                if (!TeamKey.HasValue)
                    Fetch(criteria, dal, itemPortal);

                Players.Clear();
                FieldManager.UpdateChildren(this);

                dal.Delete(criteria);
                dal.Commit(transaction);
            }
        }

        #endregion
    }
}
