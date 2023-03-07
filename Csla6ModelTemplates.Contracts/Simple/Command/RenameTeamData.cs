namespace Csla6ModelTemplates.Contracts.Simple.Command
{
    /// <summary>
    /// Defines the data access object of the rename team command.
    /// </summary>
    public class RenameTeamData
    {
        public string TeamName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the rename team command.
    /// </summary>
    public class RenameTeamDao : RenameTeamData
    {
        public long? TeamKey { get; set; }

        public RenameTeamDao(
            long? teamKey,
            string teamName
            )
        {
            TeamKey = teamKey;
            TeamName = teamName;
        }
    }

    /// <summary>
    /// Defines the data transfer object of the rename team command.
    /// </summary>
    [Serializable]
    public class RenameTeamDto : RenameTeamData
    {
        public string TeamId { get; set; }
    }
}
