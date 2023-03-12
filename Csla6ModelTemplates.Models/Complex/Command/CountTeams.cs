using Csla;
using Csla6ModelTemplates.Contracts.Complex.Command;

namespace Csla6ModelTemplates.Models.Complex.Command
{
    /// <summary>
    /// Counts the roots grouped by the number of their items.
    /// </summary>
    [Serializable]
    public class CountTeams : CommandBase<CountTeams>
    {
        #region Properties

        public static readonly PropertyInfo<string> TeamNameProperty = RegisterProperty<string>(nameof(TeamName));
        public string TeamName
        {
            get => ReadProperty(TeamNameProperty);
            private set => LoadProperty(TeamNameProperty, value);
        }

        public static readonly PropertyInfo<CountTeamsResults> ResultsProperty = RegisterProperty<CountTeamsResults>(nameof(Results));
        public CountTeamsResults Results
        {
            get => ReadProperty(ResultsProperty);
            private set => LoadProperty(ResultsProperty, value);
        }

        #endregion

        #region Business Rules

        //private void Validate()
        //{
        //    if (string.IsNullOrEmpty(TeamName))
        //        throw new CommandException(ValidationText.CountTeams_TeamName_Required);
        //}

        //private static void AddObjectAuthorizationRules()
        //{
        //    // Add authorization rules.
        //    BusinessRules.AddRule(
        //        typeof(CountTeams),
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
            CountTeamsCriteria criteria,
            [Inject] ICountTeamsDal dal,
            [Inject] IChildDataPortal<CountTeamsResults> resultPortal
            )
        {
            // Execute the command.
            TeamName = criteria.TeamName;
            //Validate();
            List<CountTeamsResultDao> list = dal.Execute(criteria);

            // Set new data.
            Results = resultPortal.FetchChild(list);
        }
        #endregion
    }
}
