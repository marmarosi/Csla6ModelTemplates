using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Complex.Edit
{
    /// <summary>
    /// Defines the editable player data.
    /// </summary>
    public class TeamPlayerData
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the editable player object.
    /// </summary>
    public class TeamPlayerDao : TeamPlayerData
    {
        public long? PlayerKey { get; set; }
        public long? TeamKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the editable player object.
    /// </summary>
    public class TeamPlayerDto : TeamPlayerData
    {
        public string PlayerId { get; set; }
        public string TeamId { get; set; }

        public TeamPlayerDao ToDao()
        {
            return new TeamPlayerDao
            {
                PlayerKey = KeyHash.Decode(ID.Player, PlayerId),
                TeamKey = KeyHash.Decode(ID.Team, TeamId),
                PlayerCode = PlayerCode,
                PlayerName = PlayerName
            };
        }
    }
}
