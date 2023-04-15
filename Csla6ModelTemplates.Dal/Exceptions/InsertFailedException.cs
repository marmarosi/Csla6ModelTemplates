using System.Runtime.Serialization;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the creation of the persistent data failed.
    /// </summary>
    [Serializable]
    public class InsertFailedException : DalException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InsertFailedException(
            string message
            )
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InsertFailedException(
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
        protected InsertFailedException(
            SerializationInfo info,
            StreamingContext context
            )
            : base(info, context)
        { }

        #endregion Constructors
    }
}
