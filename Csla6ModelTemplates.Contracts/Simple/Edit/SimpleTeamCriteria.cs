using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Simple.Edit
{
    /// <summary>
    /// Represents the criteria of the read-only team object.
    /// </summary>
    [Serializable]
    public class SimpleTeamParams
    {
        public string TeamId { get; set; }

        public SimpleTeamParams(
            string teamId
            )
        {
            TeamId = teamId;
        }

        public SimpleTeamCriteria Decode()
        {
            return new SimpleTeamCriteria(KeyHash.Decode(ID.Team, TeamId));
        }
    }

    /// <summary>
    /// Represents the criteria of the editable team object.
    /// </summary>
    [Serializable]
    public class SimpleTeamCriteria
    {
        public long? TeamKey { get; set; }

        public SimpleTeamCriteria(
            long? teamKey
            )
        {
            TeamKey = teamKey;
        }
    }
}
