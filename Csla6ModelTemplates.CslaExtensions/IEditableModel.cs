using Csla.Core;
using Csla6ModelTemplates.CslaExtensions.Validations;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Defines the helper functions of editable models.
    /// </summary>
    /// <typeparam name="Dto">The type of the data access object.</typeparam>
    public interface IEditableModel<Dto>
        where Dto : class
    {
        /// <summary>
        /// Gathers and formats broken rule messages.
        /// </summary>
        /// <param name="model">The actual business object to collect the broken rule messsages from.</param>
        /// <param name="prefix">The prefix to add property descriptions on the actual business object.</param>
        /// <param name="messages">The collection point of the formatted messages.</param>
        void CollectMessages(
            BusinessBase model,
            string prefix,
            ref List<ValidationMessage> messages
            );

        /// <summary>
        /// Converts the business object to data transfer object.
        /// </summary>
        /// <returns>The data transfer object.</returns>
        Dto ToDto();
    }
}
