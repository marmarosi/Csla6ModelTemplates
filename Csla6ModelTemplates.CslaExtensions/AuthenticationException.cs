using Csla6ModelTemplates.Dal;
using System.Net;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Represents an exception when authentication fails.
    /// </summary>
    [Serializable]
    public class AuthenticationException : BackendException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AuthenticationException(
            string message
            )
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AuthenticationException(
            string message,
            Exception innerException
            )
            : base(message, innerException)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
