using Csla6ModelTemplates.Dal.Contracts;

namespace Csla6ModelTemplates.Contracts.Arrangement.Sorting
{
    /// <summary>
    /// Represents the criteria of the read-only sorted team collection.
    /// </summary>
    [Serializable]
    public class SortedTeamListCriteria : SortedListCriteria
    {
        public string TeamName { get; set; }
    }
}
