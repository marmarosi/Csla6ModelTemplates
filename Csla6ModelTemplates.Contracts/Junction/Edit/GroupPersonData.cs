namespace Csla6ModelTemplates.Contracts.Junction.Edit
{
    /// <summary>
    /// Defines the editable group-person data.
    /// </summary>
    public class GroupPersonData
    {
        public string PersonName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the editable group-person object.
    /// </summary>
    public class GroupPersonDao : GroupPersonData
    {
        public long? PersonKey { get; set; }
        public long? GroupKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the editable group-person object.
    /// </summary>
    public class GroupPersonDto : GroupPersonData
    {
        public string PersonId { get; set; }
    }
}
