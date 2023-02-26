namespace Csla6ModelTemplates.Dal.Contracts
{
    /// <summary>
    /// Defines the read-only ID-name option data.
    /// </summary>
    public class IdNameOptionData
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only ID-name option object.
    /// </summary>
    public class IdNameOptionDao : IdNameOptionData
    {
        public long? Key { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only ID-name option object.
    /// </summary>
    public class IdNameOptionDto : IdNameOptionData
    {
        public string Id { get; set; }
    }
}
