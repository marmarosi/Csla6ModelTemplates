using System.Text.Json.Serialization;

namespace Csla6ModelTemplates.Contracts.Complex.Set
{
    /// <summary>
    /// Represents the criteria of the editable player object.
    /// </summary>
    [Serializable]
    public class TeamSetPlayerCriteria
    {
        public long? PlayerKey { get; set; }
        [JsonIgnore]
        public string __teamCode { get; set; } // for error messages
        [JsonIgnore]
        public string __playerCode { get; set; } // for error messages

        public TeamSetPlayerCriteria()
        { }

        public TeamSetPlayerCriteria(
            long? playerKey
            )
        {
            PlayerKey = playerKey;
        }
    }
}
