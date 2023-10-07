namespace Csla6ModelTemplates.Contracts.Junction.View
{
    /// <summary>
    /// Defines the read-only group data.
    /// </summary>
    public class GroupViewData
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only group object.
    /// </summary>
    public class GroupViewDao : GroupViewData
    {
        public long? GroupKey { get; set; }
        public List<GroupViewPersonDao> Persons { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the read-only group object.
    /// </summary>
    public class GroupViewDto : GroupViewData
    {
        public string GroupId { get; set; }
        public List<GroupViewPersonDto> Persons { get; set; }
    }
}
