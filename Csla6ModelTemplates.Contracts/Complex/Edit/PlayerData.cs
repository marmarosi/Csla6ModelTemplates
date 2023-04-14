using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Complex.Edit
{
    /// <summary>
    /// Defines the editable player data.
    /// </summary>
    public class PlayerData
    {
        public string PlayerCode { get; set; }
        public string PlayerName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the editable player object.
    /// </summary>
    public class PlayerDao : PlayerData
    {
        public long? PlayerKey { get; set; }
        public long? TeamKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the editable player object.
    /// </summary>
    public class PlayerDto : PlayerData
    {
        public string PlayerId { get; set; }
        public string TeamId { get; set; }

        public PlayerDao ToDao()
        {
            return new PlayerDao
            {
                PlayerKey = KeyHash.Decode(ID.Player, PlayerId),
                TeamKey = KeyHash.Decode(ID.Team, TeamId),
                PlayerCode = PlayerCode,
                PlayerName = PlayerName
            };
        }
    }
}
