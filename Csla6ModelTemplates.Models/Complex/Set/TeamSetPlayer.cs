using Csla;
using Csla.Core;
using Csla.Data;
using Csla.Rules;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.CslaExtensions.Models;
using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Models.Complex.Set
{
    /// <summary>
    /// Represents an editable player object.
    /// </summary>
    [Serializable]
    [ValidationResourceType(typeof(ValidationText), ObjectName = "Player")]
    public class TeamSetPlayer : EditableModel<TeamSetPlayer, TeamSetPlayerDto>
    {
        #region Properties

        public static readonly PropertyInfo<long?> PlayerKeyProperty = RegisterProperty<long?>(nameof(PlayerKey));
        public long? PlayerKey
        {
            get => GetProperty(PlayerKeyProperty);
            private set => SetProperty(PlayerKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> PlayerIdProperty = RegisterProperty<long?>(nameof(PlayerId), RelationshipTypes.PrivateField);
        public string PlayerId
        {
            get => KeyHash.Encode(ID.Player, PlayerKey);
            set => PlayerKey = KeyHash.Decode(ID.Player, value);
        }

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

        public static readonly PropertyInfo<string> PlayerCodeProperty = RegisterProperty<string>(nameof(PlayerCode));
        [Required]
        [MaxLength(10)]
        public string PlayerCode
        {
            get => GetProperty(PlayerCodeProperty);
            set => SetProperty(PlayerCodeProperty, value);
        }

        public static readonly PropertyInfo<string> PlayerNameProperty = RegisterProperty<string>(nameof(PlayerName));
        [Required]
        [MaxLength(100)]
        public string PlayerName
        {
            get => GetProperty(PlayerNameProperty);
            set => SetProperty(PlayerNameProperty, value);
        }

        #endregion

        #region Business Rules

        protected override void AddBusinessRules()
        {
            // Call base class implementation to add data annotation rules to BusinessRules.
            // NOTE: DataAnnotation rules is always added with Priority = 0.
            base.AddBusinessRules();

            // Add validation rules.
            BusinessRules.AddRule(new UniquePlayerCodes(PlayerCodeProperty));

            //// Add authorization rules.
            //BusinessRules.AddRule(
            //    new IsInRole(
            //        AuthorizationActions.WriteProperty,
            //        PlayerCodeProperty,
            //        "Manager"
            //        )
            //    );
        }

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(TeamSetPlayer),
        //        new IsInRole(
        //            AuthorizationActions.EditObject,
        //            "Manager"
        //            )
        //        );
        //}

        private sealed class UniquePlayerCodes : BusinessRule
        {
            // Add additional parameters to your rule to the constructor.
            public UniquePlayerCodes(
                IPropertyInfo primaryProperty
                )
              : base(primaryProperty)
            {
                // If you are  going to add InputProperties make sure to
                // uncomment line below as InputProperties is NULL by default.
                //if (InputProperties == null) InputProperties = new List<IPropertyInfo>();

                // Add additional constructor code here.

                // Marke rule for IsAsync if Execute contains asyncronous code IsAsync = true; 
            }

            protected override void Execute(
                IRuleContext context
                )
            {
                TeamSetPlayer target = (TeamSetPlayer)context.Target;
                if (target.Parent == null)
                    return;

                TeamSetItem team = (TeamSetItem)target.Parent.Parent;
                var count = team.Players.Count(player => player.PlayerCode == target.PlayerCode);
                if (count > 1)
                    context.AddErrorResult(ValidationText.Player_PlayerCode_NotUnique);
            }
        }

        #endregion

        #region Business Methods

        /// <summary>
        /// Updates an editable model and its children from the data transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        public override void SetValuesOnBuild(
            TeamSetPlayerDto dto,
            IChildDataPortalFactory childFactory
            )
        {
            DataMapper.Map(dto, this);
            BusinessRules.CheckRules();
        }

        #endregion

        #region Data Access

        [CreateChild]
        private void Create()
        {
            // Set values from data transfer object.
            //LoadProperty(TeamCodeProperty, "");
            //BusinessRules.CheckRules();
        }            

        [FetchChild]
        private void Fetch(
            TeamSetPlayerDao dao
            )
        {
            // Load values from persistent storage.
            using (BypassPropertyChecks)
                DataMapper.Map(dao, this);
        }

        [InsertChild]
        private void Child_Insert(
            [Inject] ITeamSetPlayerDal dal
            )
        {
            // Insert values into persistent storage.
            using (BypassPropertyChecks)
            {
                var dao = Copy.PropertiesFrom(this).ToNew<TeamSetPlayerDao>();
                dal.Insert(dao);

                // Set new data.
                PlayerKey = dao.PlayerKey;
            }
            //FieldManager.UpdateChildren(this);
        }

        [UpdateChild]
        private void Update(
            [Inject] ITeamSetPlayerDal dal
            )
        {
            // Update values in persistent storage.
            using (BypassPropertyChecks)
            {
                var dao = Copy.PropertiesFrom(this).ToNew<TeamSetPlayerDao>();
                dal.Update(dao);

                // Set new data.
            }
            //FieldManager.UpdateChildren(this);
        }

        [DeleteSelfChild]
        private void Child_DeleteSelf(
            [Inject] ITeamSetPlayerDal dal
            )
        {
            // Delete values from persistent storage.

            //Items.Clear();
            //FieldManager.UpdateChildren(this);

            TeamSetPlayerCriteria criteria = new TeamSetPlayerCriteria(PlayerKey)
            {
                __teamCode = ((TeamSetItem)Parent.Parent).TeamCode,
                __playerCode = PlayerCode
            };
            dal.Delete(criteria);
        }

        #endregion
    }
}
