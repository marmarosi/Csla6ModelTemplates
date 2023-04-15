namespace Csla6ModelTemplates.Contracts.Arrangement.Sorting
{
    /// <summary>
    /// Defines the data access functions of the read-only sorted team collection.
    /// </summary>
    public interface ISortedTeamListDal
    {
        List<SortedTeamListItemDao> Fetch(SortedTeamListCriteria criteria);
    }
}
