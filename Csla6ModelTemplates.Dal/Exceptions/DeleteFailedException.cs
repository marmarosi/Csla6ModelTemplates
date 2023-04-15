using System.Runtime.Serialization;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the deletion of the persistent data failed.
    /// </summary>
    [Serializable]
    public class DeleteFailedException : DalException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DeleteFailedException(
            string message
            )
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DeleteFailedException(
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
        protected DeleteFailedException(
            SerializationInfo info,
            StreamingContext context
            )
            : base(info, context)
        { }

        #endregion Constructors
    }
}
