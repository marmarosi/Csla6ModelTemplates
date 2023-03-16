using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Selection.WithKey
{
    /// <summary>
    /// Represents the criteria of the read-only team choice collection.
    /// </summary>
    [Serializable]
    public class TeamKeyChoiceCriteria : ChoiceCriteria
    {
        public string TeamName { get; set; }
    }
}
