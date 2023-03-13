namespace Csla6ModelTemplates.Contracts.Complex.Edit
{
    /// <summary>
    /// Represents the criteria of the editable player object.
    /// </summary>
    [Serializable]
    public class PlayerCriteria
    {
        public long? PlayerKey { get; set; }

        public PlayerCriteria()
        { }

        public PlayerCriteria(
            long? playerKey
            )
        {
            PlayerKey = playerKey;
        }
    }
}
