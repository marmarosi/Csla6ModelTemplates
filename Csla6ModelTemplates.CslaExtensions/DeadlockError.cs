using Csla6ModelTemplates.Dal;
using System.Runtime.Serialization;

namespace Csla6ModelTemplates.CslaExtensions
{
    public class DeadlockError : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public DeadlockError() { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">The message to send to the client.</param>
        public DeadlockError(
            string message
            ) : base(message)
        { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">The message to send to the client.</param>
        /// <param name="name">The name of the error type.</param>
        public DeadlockError(
            string message,
            Exception innerException
            ) : base(message, innerException)
        { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="info">Holds the serialized onject data about the excetion being thrown.</param>
        /// <param name="context">Contains contextual information about the source or destination.</param>
        public DeadlockError(
            SerializationInfo info,
            StreamingContext context
            ) : base(info, context)
        { }

        #endregion

        public static DeadlockError CheckException(
            Exception exception
            )
        {
            Exception firstEx = exception;
            while (firstEx.InnerException != null)
                firstEx = firstEx.InnerException;

            return DalFactory.HasDeadlock(firstEx)
                ? new DeadlockError(firstEx.Message)
                : null;
        }
    }
}
