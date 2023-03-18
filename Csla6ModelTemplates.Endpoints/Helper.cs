using Csla6ModelTemplates.CslaExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.Endpoints
{
    /// <summary>
    /// Provides methods to handle errors.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Gets the path of the request.
        /// </summary>
        public static string Uri(
            HttpRequest request
            )
        {
            return request == null ? "" : request.Path.ToString();
        }

        /// <summary>
        /// Handles the eventual exceptions.
        /// </summary>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="logger">The application logging service.</param>
        /// <param name="exception">The exception thrown by the backend.</param>
        /// <returns>The error information to send to the frontend.</returns>
        public static ObjectResult HandleError(
            ControllerBase endpoint,
            ILogger logger,
            Exception exception
            )
        {
            ObjectResult result = null;

            // Check validation exception.
            if (exception is ValidationException)
                // Status code 422 = Unprocesssable Entity
                return endpoint.UnprocessableEntity(
                    new ValidationError(exception as ValidationException)
                    );

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

            logger.LogError(exception, backend.Summary, null);

            result = new ObjectResult(backend);
            result.StatusCode = statusCode;
            return result;
        }
    }
}
