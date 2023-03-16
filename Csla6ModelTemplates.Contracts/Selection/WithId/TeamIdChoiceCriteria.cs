using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Selection.WithId
{
    /// <summary>
    /// Represents the criteria of the read-only team choice collection.
    /// </summary>
    [Serializable]
    public class TeamIdChoiceCriteria : ChoiceCriteria
    {
        public string TeamName { get; set; }
    }
}
