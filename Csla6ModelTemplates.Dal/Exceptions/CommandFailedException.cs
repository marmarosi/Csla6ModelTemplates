using System;
using System.Net;

namespace Csla6ModelTemplates.Dal.Exceptions
{
    /// <summary>
    /// Represents an exception when the execution of a command failed.
    /// </summary>
    [Serializable]
    public class CommandFailedException : DalException
    {
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
    }
}
