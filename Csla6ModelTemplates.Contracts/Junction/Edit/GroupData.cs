namespace Csla6ModelTemplates.Contracts.Junction.Edit
{
    /// <summary>
    /// Defines the editable group data.
    /// </summary>
    public class GroupData
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the editable group object.
    /// </summary>
    public class GroupDao : GroupData
    {
        public long? GroupKey { get; set; }
        public List<GroupPersonDao> Persons { get; set; }

        public GroupDao()
        {
            Persons = new List<GroupPersonDao>();
        }
    }

    /// <summary>
    /// Defines the data transfer object of the editable group object.
    /// </summary>
    public class GroupDto : GroupData
    {
        public string GroupId { get; set; }
        public List<GroupPersonDto> Persons { get; set; }

        public GroupDto()
        {
            Persons = new List<GroupPersonDto>();
        }
    }
}
