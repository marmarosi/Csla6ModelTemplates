namespace Csla6ModelTemplates.Contracts.Junction.View
{
    /// <summary>
    /// Defines the read-only group-person data.
    /// </summary>
    public class GroupPersonViewData
    {
        public string PersonName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only group-person object.
    /// </summary>
    public class GroupPersonViewDao : GroupPersonViewData
    {
        public long? PersonKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only group-person object.
    /// </summary>
    public class GroupPersonViewDto : GroupPersonViewData
    {
        public string PersonId { get; set; }
    }
}
