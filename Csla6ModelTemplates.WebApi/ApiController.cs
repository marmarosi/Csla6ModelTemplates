using Csla;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi
{
    /// <summary>
    /// Serves as the base class for the API controllers.
    /// </summary>
    public class ApiController : ControllerBase
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }
        internal IChildDataPortalFactory ChildFactory { get; private set; }
        internal IDeadLockDetector DeadLock { get; private set; }

        /// <summary>
        /// Gets the path of the request.
        /// </summary>
        protected string Uri
        {
            get { return Request == null ? "" : Request.Path.ToString(); }
        }

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        internal ApiController(
            ILogger logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Factory = csla.Factory;
            ChildFactory = csla.ChildFactory;
            DeadLock = csla.DeadLock;
        }

        /// <summary>
        /// Handles the eventual exceptions.
        /// </summary>
        /// <param name="exception">The exception thrown by the backend.</param>
        /// <returns>The error information to send to the frontend.</returns>
        protected ObjectResult HandleError(
            Exception exception
            )
        {
            ObjectResult result = null;

            // Check validation exception.
            if (exception is ValidationException vException)
                // Status code 422 = Unprocesssable Entity
                return StatusCode(422, new ValidationError(vException));

            // Check deadlock exception.
            DeadlockException deadlock = DeadLock.CheckException(exception);
            if (deadlock != null)
            {
                // Status code 423 = Locked
                result = new ObjectResult(deadlock);
                result.StatusCode = StatusCodes.Status423Locked;
                return result;
            }

            // Evaluate other exceptions.
            int statusCode;
            BackendError backend = BackendError.Evaluate(exception, out statusCode);

            Logger.LogError(exception, backend.Summary, null);

            result = new ObjectResult(backend);
            result.StatusCode = statusCode;
            return result;
        }
    }
}
