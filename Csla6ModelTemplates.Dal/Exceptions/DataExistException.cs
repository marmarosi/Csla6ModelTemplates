using System;
using System.Net;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the requested persistent data already exist.
    /// </summary>
    [Serializable]
    public class DataExistException : DalException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataExistException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DataExistException(
            string message
            )
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.Conflict;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataExistException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DataExistException(
            string message,
            Exception innerException
            )
            : base(message, innerException)
        {
            StatusCode = (int)HttpStatusCode.Conflict;
        }
    }
}
