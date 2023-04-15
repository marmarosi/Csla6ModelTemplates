using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Selection.WithCode
{
    /// <summary>
    /// Represents the criteria of the read-only team choice collection.
    /// </summary>
    [Serializable]
    public class TeamCodeChoiceCriteria : ChoiceCriteria
    {
        public string TeamName { get; set; }
    }
}
