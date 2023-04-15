using System.Runtime.Serialization;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the operation cannot be executed for some wrong data.
    /// </summary>
    [Serializable]
    public class InvalidDataException : DalException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidDataException(
            string message
            )
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidDataException(
            string message,
            Exception innerException
            )
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info">The serialization info that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The streaming context that contains contextual information about the source or destination.</param>
        protected InvalidDataException(
            SerializationInfo info,
            StreamingContext context
            )
            : base(info, context)
        { }

        #endregion Constructors
    }
}
