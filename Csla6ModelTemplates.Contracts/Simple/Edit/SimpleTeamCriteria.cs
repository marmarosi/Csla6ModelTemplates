namespace Csla6ModelTemplates.Contracts.Simple.Edit
{
    /// <summary>
    /// Represents the criteria of the read-only team object.
    /// </summary>
    [Serializable]
    public class SimpleTeamParams
    {
        public string TeamId { get; set; }

        public SimpleTeamCriteria Decode()
        {
            return new SimpleTeamCriteria
            {
                TeamKey = KeyHash.Decode(ID.Team, TeamId) ?? 0
            };
        }
    }

    /// <summary>
    /// Represents the criteria of the editable team object.
    /// </summary>
    [Serializable]
    public class SimpleTeamCriteria
    {
        public long TeamKey { get; set; }

        public SimpleTeamCriteria() { }

        public SimpleTeamCriteria(
            long teamKey
            )
        {
            TeamKey = teamKey;
        }
    }
}
