using Csla;

namespace Csla6ModelTemplates.CslaExtensions
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
        /// <param name="childFactory">The data portal factory of the items.</param>
        void SetValuesByKey
            (List<Dto> list,
            string keyName,
            IChildDataPortalFactory childFactory
            );

        /// <summary>
        /// Updates an editable collection from the data transfer objects.
        /// </summary>
        /// <param name="list">The list of data transfer objects.</param>
        /// <param name="idName">The name of the identifier property.</param>
        /// <param name="childFactory">The data portal factory of the items.</param>
        void SetValuesById(
            List<Dto> list,
            string idName,
            IChildDataPortalFactory childFactory
            );
    }
}
