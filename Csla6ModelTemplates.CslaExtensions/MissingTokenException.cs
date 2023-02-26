using Csla6ModelTemplates.Dal;
using System.Net;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Represents an exception when the JWT token is not found in the token cache.
    /// </summary>
    [Serializable]
    public class MissingTokenException : BackendException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTokenException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MissingTokenException(
            string message
            )
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingTokenException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MissingTokenException(
            string message,
            Exception innerException
            )
            : base(message, innerException)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
