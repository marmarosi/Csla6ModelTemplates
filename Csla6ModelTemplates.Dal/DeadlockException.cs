using System.Runtime.Serialization;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Represents an exception thrown by the data access layer.
    /// </summary>
    [Serializable]
    public class DeadlockException : BackendException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message">The message.</param>
        public DeadlockException(
            string message
            )
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DeadlockException(
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
        protected DeadlockException(
            SerializationInfo info,
            StreamingContext context
            )
            : base(info, context)
        { }

        #endregion Constructors
    }
}
