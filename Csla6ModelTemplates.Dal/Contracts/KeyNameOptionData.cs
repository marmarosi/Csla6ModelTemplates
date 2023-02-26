namespace Csla6ModelTemplates.Dal.Contracts
{
    /// <summary>
    /// Defines the read-only key-name option data.
    /// </summary>
    public class KeyNameOptionData
    {
        public long? Key { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only key-name option object.
    /// </summary>
    public class KeyNameOptionDao : KeyNameOptionData
    { }

    /// <summary>
    /// Defines the data transfer object of the read-only key-name option object.
    /// </summary>
    public class KeyNameOptionDto : KeyNameOptionData
    { }
}
