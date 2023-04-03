using System.Net;
using System.Runtime.Serialization;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the requested persistent data are not found.
    /// </summary>
    [Serializable]
    public class DataNotFoundException : DalException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DataNotFoundException(
            string message
            )
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DataNotFoundException(
            string message,
            Exception innerException
            )
            : base(message, innerException)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info">The serialization info that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The streaming context that contains contextual information about the source or destination.</param>
        protected DataNotFoundException(
            SerializationInfo info,
            StreamingContext context
            )
            : base(info, context)
        { }

        #endregion Constructors
    }
}
