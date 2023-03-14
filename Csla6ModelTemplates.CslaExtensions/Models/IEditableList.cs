using Csla;

namespace Csla6ModelTemplates.CslaExtensions.Models
{
    /// <summary>
    /// Defines the helper functions of editable lists.
    /// </summary>
    /// <typeparam name="Dto">The type of the data access object.</typeparam>
    public interface IEditableList<Dto, C>
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
        /// <param name="itemPortal">The data portal of the items.</param>
        void UpdateByKey
            (List<Dto> list,
            string keyName,
            IChildDataPortal<C> itemPortal
            );

        /// <summary>
        /// Updates an editable collection from the data transfer objects.
        /// </summary>
        /// <param name="list">The list of data transfer objects.</param>
        /// <param name="idName">The name of the identifier property.</param>
        /// <param name="itemPortal">The data portal of the items.</param>
        void UpdateById(
            List<Dto> list,
            string idName,
            IChildDataPortal<C> itemPortal
            );
    }
}
