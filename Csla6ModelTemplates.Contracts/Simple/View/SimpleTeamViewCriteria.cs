namespace Csla6ModelTemplates.Contracts.Simple.View
{
    /// <summary>
    /// Represents the criteria of the read-only team model.
    /// </summary>
    [Serializable]
    public class SimpleTeamViewParams
    {
        public string TeamId { get; set; }

        public SimpleTeamViewParams(
            string teamId
            )
        {
            TeamId = teamId;
        }

        public SimpleTeamViewCriteria Decode()
        {
            return new SimpleTeamViewCriteria
            {
                TeamKey = KeyHash.Decode(ID.Team, TeamId)
            };
        }
    }

    /// <summary>
    /// Represents the criteria of the read-only team object.
    /// </summary>
    [Serializable]
    public class SimpleTeamViewCriteria
    {
        public long? TeamKey { get; set; }
    }
}
