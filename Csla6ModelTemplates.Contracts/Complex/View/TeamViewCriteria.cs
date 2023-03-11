namespace Csla6ModelTemplates.Contracts.Complex.View
{
    /// <summary>
    /// Represents the criteria of the read-only team object.
    /// </summary>
    [Serializable]
    public class TeamViewParams
    {
        public string TeamId { get; set; }

        public TeamViewParams(
            string teamId
            )
        {
            TeamId = teamId;
        }

        public TeamViewCriteria Decode()
        {
            return new TeamViewCriteria
            {
                TeamKey = KeyHash.Decode(ID.Team, TeamId) ?? 0
            };
        }
    }

    /// <summary>
    /// Represents the criteria of the read-only team object.
    /// </summary>
    [Serializable]
    public class TeamViewCriteria
    {
        public long TeamKey { get; set; }
    }
}
