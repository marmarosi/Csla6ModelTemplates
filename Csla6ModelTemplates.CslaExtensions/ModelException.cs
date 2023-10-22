using Csla6ModelTemplates.Dal;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Represents an exception thrown by a business object for failed business logic.
    /// </summary>
    public class ModelException : BackendException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ModelException(
            string message
            )
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ModelException(
            string message,
            Exception innerException
            )
            : base(message, innerException)
        { }
    }
}
