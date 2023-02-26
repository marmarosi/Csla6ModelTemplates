using System;
using System.Net;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the requested persistent data has ben changed.
    /// </summary>
    [Serializable]
    public class ConcurrencyException : DalException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ConcurrencyException(
            string message
            )
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.Conflict;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConcurrencyException(
            string message,
            Exception innerException
            )
            : base(message, innerException)
        {
            StatusCode = (int)HttpStatusCode.Conflict;
        }
    }
}
