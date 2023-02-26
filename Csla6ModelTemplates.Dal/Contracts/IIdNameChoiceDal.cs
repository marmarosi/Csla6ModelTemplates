using System.Collections.Generic;

namespace Csla6ModelTemplates.Dal.Contracts
{
    /// <summary>
    /// Defines the data access functions of the read-only ID-name choice object.
    /// </summary>
    public interface IIdNameChoiceDal<T> : IDal
        where T : ChoiceCriteria
    {
        List<IdNameOptionDao> Fetch(T criteria);
    }
}
