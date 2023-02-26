using System;

namespace Csla6ModelTemplates.Dal.Contracts
{
    /// <summary>
    /// Represents the base criteria of the read-only sorted collections.
    /// </summary>
    [Serializable]
    public class SortedListCriteria
    {
        /// <summary>
        /// Specifies the properties to sort a list of items by.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Specifies the direction in which to sort a list of items.
        /// </summary>
        public SortDirection SortDirection { get; set; }
    }
}
