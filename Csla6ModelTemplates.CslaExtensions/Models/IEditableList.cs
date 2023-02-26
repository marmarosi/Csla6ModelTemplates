using System.Collections.Generic;
using System.Threading.Tasks;

namespace Csla6ModelTemplates.CslaExtensions.Models
{
    /// <summary>
    /// Defines the helper functions of editable lists.
    /// </summary>
    /// <typeparam name="Dto">The type of the data access object.</typeparam>
    public interface IEditableList<Dto>
        where Dto : class
    {
        /// <summary>
        /// Converts the business collection to data transfer object list.
        /// </summary>
        /// <returns>The list of the data transfer objects.</returns>
        IList<Dto> ToDto();

        /// <summary>
        /// Updates an editable collection from the data transfer objects.
        /// </summary>
        /// <param name="list">The list of data transfer objects.</param>
        /// <param name="keyName">The name of the key property.</param>
        /// <returns></returns>
        //Task Update(List<Dto> list, string keyName);

        /// <summary>
        /// Updates an editable collection from the data transfer objects.
        /// </summary>
        /// <param name="list">The list of data transfer objects.</param>
        /// <param name="idName">The name of the identifier property.</param>
        Task Update(List<Dto> list, string idName);
    }
}
