using Csla6ModelTemplates.Dal.Contracts;
using System.Text.Json.Serialization;

namespace Csla6ModelTemplates.Contracts.Complex.Set
{
    /// <summary>
    /// Defines the editable team set player data.
    /// </summary>
    public class TeamSetPlayerData
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
        [JsonIgnore]
        public string __teamCode; // for error messages
    }

    /// <summary>
    /// Defines the data access object of the editable team set player object.
    /// </summary>
    public class TeamSetPlayerDao : TeamSetPlayerData
    {
        public long? PlayerKey { get; set; }
        public long? TeamKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the editable team set player object.
    /// </summary>
    public class TeamSetPlayerDto : TeamSetPlayerData
    {
        public string PlayerId { get; set; }
        public string TeamId { get; set; }

        public TeamSetPlayerDao ToDao()
        {
            return new TeamSetPlayerDao
            {
                PlayerKey = KeyHash.Decode(ID.Player, PlayerId),
                TeamKey = KeyHash.Decode(ID.Team, TeamId),
                PlayerCode = PlayerCode,
                PlayerName = PlayerName,
                __teamCode = __teamCode
            };
        }

    }
}
