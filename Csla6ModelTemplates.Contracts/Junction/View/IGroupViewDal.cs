namespace Csla6ModelTemplates.Contracts.Junction.View
{
    /// <summary>
    /// Defines the data access functions of the read-only group object.
    /// </summary>
    public interface IGroupViewDal
    {
        GroupViewDao Fetch(GroupViewCriteria criteria);
    }
}
