using Csla6ModelTemplates.CslaExtensions.Validations;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Represents a validation error.
    /// </summary>
    [Serializable]
    public class ValidationError
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the error type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the error messages to send to the client.
        /// </summary>
        public List<ValidationMessage> Messages { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="exception">The validation exception.</param>
        public ValidationError(
            ValidationException exception
            )
        {
            Name = exception.GetType().Name;
            Messages = exception.Messages;
        }

        #endregion
    }
}
