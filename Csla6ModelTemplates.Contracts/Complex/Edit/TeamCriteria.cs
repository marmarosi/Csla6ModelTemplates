namespace Csla6ModelTemplates.Contracts.Complex.Edit
{
    /// <summary>
    /// Represents the criteria of the editable team object.
    /// </summary>
    [Serializable]
    public class TeamParams
    {
        public string TeamId { get; set; }

        public TeamParams(
            string teamId
            )
        {
            TeamId = teamId;
        }

        public TeamCriteria Decode()
        {
            return new TeamCriteria
            {
                TeamKey = KeyHash.Decode(ID.Team, TeamId) ?? 0
            };
        }
    }

    /// <summary>
    /// Represents the criteria of the editable team object.
    /// </summary>
    [Serializable]
    public class TeamCriteria
    {
        public long? TeamKey { get; set; }

        public TeamCriteria() { }

        public TeamCriteria(
            long? teamKey
            )
        {
            TeamKey = teamKey;
        }
    }
}
