using System;
using System.Net;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the requested persistent data are not found.
    /// </summary>
    [Serializable]
    public class DataNotFoundException : DalException
    {
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
    }
}
