using Csla;
using Csla6ModelTemplates.Contracts;
using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Models.Simple.Command
{
    /// <summary>
    /// Renames the specified team.
    /// </summary>
    [Serializable]
    public class RenameTeam : CommandBase<RenameTeam>
    {
        #region Properties

        public static readonly PropertyInfo<long?> TeamKeyProperty = RegisterProperty<long?>(nameof(TeamKey));
        public long? TeamKey
        {
            get => ReadProperty(TeamKeyProperty);
            private set => LoadProperty(TeamKeyProperty, value);
        }

        public static readonly PropertyInfo<long?> TeamIdProperty = RegisterProperty<long?>(nameof(TeamId), RelationshipTypes.PrivateField);
        public string TeamId
        {
            get => KeyHash.Encode(ID.Team, TeamKey);
            set => TeamKey = KeyHash.Decode(ID.Team, value);
        }

        public static readonly PropertyInfo<string> TeamNameProperty = RegisterProperty<string>(c => c.TeamName);
        public string TeamName
        {
            get => ReadProperty(TeamNameProperty);
            private set => LoadProperty(TeamNameProperty, value);
        }

        public static readonly PropertyInfo<bool> ResultProperty = RegisterProperty<bool>(c => c.Result);
        public bool Result
        {
            get => ReadProperty(ResultProperty);
            private set => LoadProperty(ResultProperty, value);
        }

        #endregion

        #region Business Rules

        private void Validate()
        {
            if (string.IsNullOrEmpty(TeamName))
                throw new CommandException(ValidationText.RenameTeam_TeamName_Required);
        }

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(RenameTeam),
        //        new IsInRole(
        //            AuthorizationActions.ExecuteMethod,
        //            "Manager"
        //            )
        //        );
        //}

        #endregion

        #region Data Access

        [Execute]
        private void Execute(
            RenameTeamDto dto,
            [Inject] IRenameTeamDal dal
            )
        {
            // Execute the command.
            TeamId = dto.TeamId;
            TeamName = dto.TeamName;
            Validate();

            using (var transaction = dal.BeginTransaction())
            {
                RenameTeamDao dao = new RenameTeamDao(TeamKey ?? 0, TeamName);
                dal.Execute(dao);
            }

            // Set new data.
            Result = true;
        }

        #endregion
    }
}
