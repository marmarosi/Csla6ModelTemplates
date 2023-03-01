namespace Csla6ModelTemplates.Contracts.Simple.Edit
{
    /// <summary>
    /// Defines the editable team data.
    /// </summary>
    public class SimpleTeamData
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the editable team object.
    /// </summary>
    public class SimpleTeamDao : SimpleTeamData
    {
        public long? TeamKey { get; set; }
    }

    /// <summary>
    /// Defines the data transfer object of the editable team object.
    /// </summary>
    public class SimpleTeamDto : SimpleTeamData
    {
        public string TeamId { get; set; }
    }
}
