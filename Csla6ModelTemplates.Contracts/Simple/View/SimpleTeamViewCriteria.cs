namespace Csla6ModelTemplates.Contracts.Simple.View
{
    /// <summary>
    /// Represents the criteria of the read-only team object.
    /// </summary>
    [Serializable]
    public class SimpleTeamViewParams
    {
        public string TeamId { get; set; }

        public SimpleTeamViewCriteria Decode()
        {
            return new SimpleTeamViewCriteria
            {
                TeamKey = KeyHash.Decode(ID.Team, TeamId) ?? 0
            };
        }
    }

    /// <summary>
    /// Represents the criteria of the read-only team object.
    /// </summary>
    [Serializable]
    public class SimpleTeamViewCriteria
    {
        public long TeamKey { get; set; }
    }
}
