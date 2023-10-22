using Csla;
using Csla.Data;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Models.Complex.Set
{
    /// <summary>
    /// Represents an editable team object.
    /// </summary>
    [Serializable]
    [ValidationResourceType(typeof(ValidationText), ObjectName = "TeamSetItem")]
    public class TeamSetItem : EditableModel<TeamSetItem, TeamSetItemDto>
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

        public static readonly PropertyInfo<TeamSetPlayers> PlayersProperty = RegisterProperty<TeamSetPlayers>(nameof(Players));
        public TeamSetPlayers Players
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
        //        typeof(TeamSetItem),
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
            TeamSetItemDto dto,
            IChildDataPortalFactory childFactory
            )
        {
            DataMapper.Map(dto, this, "Players");
            BusinessRules.CheckRules();
            Players.SetValuesById(dto.Players, "PlayerId", childFactory);
        }

        #endregion

        #region Data Access

        [CreateChild]
        private void Create(
            [Inject] IChildDataPortal<TeamSetPlayers> itemsPortal
            )
        {
            // Set values from data transfer object.
            Players = itemsPortal.CreateChild();
            //LoadProperty(TeamCodeProperty, "");
            //BusinessRules.CheckRules();
        }

        [FetchChild]
        private void Fetch(
            TeamSetItemDao dao,
            [Inject] IChildDataPortal<TeamSetPlayers> itemsPortal
            )
        {
            // Set values from data access object.
            using (BypassPropertyChecks)
            {
                DataMapper.Map(dao, this, "Players");
                Players = itemsPortal.FetchChild(dao.Players);
            }
        }

        [InsertChild]
        private void Insert(
            [Inject] ITeamSetItemDal dal
            )
        {
            // Insert values into persistent storage.
            using (BypassPropertyChecks)
            {
                var dao = Copy.PropertiesFrom(this).Omit("Players").ToNew<TeamSetItemDao>();
                dal.Insert(dao);

                // Set new data.
                TeamKey = dao.TeamKey;
                Timestamp = dao.Timestamp;
            }
            FieldManager.UpdateChildren(this);
        }

        [UpdateChild]
        private void Update(
            [Inject] ITeamSetItemDal dal
            )
        {
            // Update values in persistent storage.
            if (IsSelfDirty)
            {
                using (BypassPropertyChecks)
                {
                    var dao = Copy.PropertiesFrom(this).Omit("Players").ToNew<TeamSetItemDao>();
                    dal.Update(dao);

                    // Set new data.
                    Timestamp = dao.Timestamp;
                }
            }
            FieldManager.UpdateChildren(this);
        }

        [DeleteSelfChild]
        private void DeleteSelf(
            [Inject] ITeamSetItemDal dal
            )
        {
            // Delete values from persistent storage.
            if (TeamKey.HasValue)
            {
                Players.Clear();
                FieldManager.UpdateChildren(this);

                var criteria = new TeamSetItemCriteria(TeamKey);
                dal.Delete(criteria);
            }
        }

        #endregion
    }
}
