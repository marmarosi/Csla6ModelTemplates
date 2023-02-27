using System.Collections.Generic;

namespace Csla6ModelTemplates.Dal.Contracts
{
    /// <summary>
    /// Defines the data access functions of the read-only code-name choice object.
    /// </summary>
    public interface ICodeNameChoiceDal<T>
        where T : ChoiceCriteria
    {
        List<CodeNameOptionDao> Fetch(T criteria);
    }
}
