using System.Net;
using System.Runtime.Serialization;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the execution of a command failed.
    /// </summary>
    [Serializable]
    public class CommandFailedException : DalException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CommandFailedException(
            string message
            )
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CommandFailedException(
            string message,
            Exception innerException
            )
            : base(message, innerException)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info">The serialization info that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The streaming context that contains contextual information about the source or destination.</param>
        protected CommandFailedException(
            SerializationInfo info,
            StreamingContext context
            )
            : base(info, context)
        { }

        #endregion Constructors
    }
}
