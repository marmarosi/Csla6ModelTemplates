namespace Csla6ModelTemplates.Contracts.Complex.Edit
{
    /// <summary>
    /// Represents the criteria of the editable player object.
    /// </summary>
    [Serializable]
    public class TeamPlayerCriteria
    {
        public long? PlayerKey { get; set; }

        public TeamPlayerCriteria()
        { }

        public TeamPlayerCriteria(
            long? playerKey
            )
        {
            PlayerKey = playerKey;
        }
    }
}
