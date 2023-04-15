namespace Csla6ModelTemplates.Contracts.Complex.Command
{
    /// <summary>
    /// Represents the criteria of the count teams by player count command.
    /// </summary>
    [Serializable]
    public class CountTeamsCriteria
    {
        public string TeamName { get; set; }

        public CountTeamsCriteria()
        { }

        public CountTeamsCriteria(
            string teamName
            )
        {
            TeamName = teamName;
        }
    }
}
