using Csla6ModelTemplates.CslaExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi
{
    /// <summary>
    /// Serves as the base class for the API controllers.
    /// </summary>
    public class ApiController : ControllerBase
    {
        private const int MAX_RETRIES = 1;
        private const int MIN_DELAY_MS = 100;
        private const int MAX_DELAY_MS = 200;

        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        internal ILogger Logger { get; set; }

        /// <summary>
        /// Gets the path of the request.
        /// </summary>
        protected string Uri
        {
            get
            {
                return Request == null ? "" : Request.Path.ToString();
            }
        }

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        internal ApiController(
            ILogger logger
            )
        {
            Logger = logger;
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
            if (exception is ValidationException)
                // Status code 422 = Unprocesssable Entity
                return StatusCode(422, new ValidationError((ValidationException)exception));

            // Check deadlock exception.
            DeadlockError deadlock = DeadlockError.CheckException(exception);
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
