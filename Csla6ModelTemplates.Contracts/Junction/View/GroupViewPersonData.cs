namespace Csla6ModelTemplates.Contracts.Junction.View
{
    /// <summary>
    /// Defines the read-only group-person data.
    /// </summary>
    public class GroupViewPersonData
    {
        public string PersonName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only group-person object.
    /// </summary>
    public class GroupViewPersonDao : GroupViewPersonData
    {
        public long? PersonKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only group-person object.
    /// </summary>
    public class GroupViewPersonDto : GroupViewPersonData
    {
        public string PersonId { get; set; }
    }
}
