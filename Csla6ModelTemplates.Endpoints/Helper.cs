using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.Endpoints
{
    /// <summary>
    /// Provides methods to handle errors.
    /// </summary>
    public static class Helper
    {
        internal const int MAX_RETRIES = 1;
        internal const int MIN_DELAY_MS = 200;
        internal const int MAX_DELAY_MS = 400;

        /// <summary>
        /// Gets the path of the request.
        /// </summary>
        public static string Uri(
            HttpRequest request
            )
        {
            return request == null ? "" : request.Path.ToString();
        }

        #region RetryOnDeadlock

        /// <summary>
        /// Executes a function, and retries when it fails due to deadlock.
        /// </summary>
        /// <param name="businessMethod">The function to execute.</param>
        /// <param name="maxRetries">The number of attempts, defaults to 3.</param>
        /// <returns>The result of the action.</returns>
        public static async Task RetryOnDeadlock(
            Func<Task> businessMethod,
            int maxRetries = MAX_RETRIES
            )
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    await businessMethod();
                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (ex is DeadlockException && retryCount <= maxRetries)
                    {
                        Thread.Sleep(RandomInt.Next(MIN_DELAY_MS, MAX_DELAY_MS));
                    }
                    else
                        throw;
                }
            }
        }

        /// <summary>
        /// Executes a function, and retries when it fails due to deadlock.
        /// </summary>
        /// <param name="businessMethod">The function to execute.</param>
        /// <param name="maxRetries">The number of attempts, defaults to 3.</param>
        /// <returns>The result of the action.</returns>
        public static async Task<object> RetryOnDeadlock(
            Func<Task<object>> businessMethod,
            int maxRetries = MAX_RETRIES
            )
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    return await businessMethod();
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (ex is DeadlockException && retryCount <= maxRetries)
                    {
                        Thread.Sleep(RandomInt.Next(MIN_DELAY_MS, MAX_DELAY_MS));
                    }
                    else
                        throw;
                }
            }
        }

        #endregion

        /// <summary>
        /// Handles the eventual exceptions.
        /// </summary>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="logger">The application logging service.</param>
        /// <param name="deadLock">The dead lock detector service.</param>
        /// <param name="exception">The exception thrown by the backend.</param>
        /// <returns>The error information to send to the frontend.</returns>
        public static ObjectResult HandleError_Old(
            ControllerBase endpoint,
            ILogger logger,
            IDeadLockDetector deadLock,
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
            DeadlockException deadlock = deadLock.CheckException(exception);
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

        /// <summary>
        /// Handles the eventual exceptions.
        /// </summary>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="logger">The application logging service.</param>
        /// <param name="deadLock">The dead lock detector service.</param>
        /// <param name="exception">The exception thrown by the backend.</param>
        /// <returns>The error information to send to the frontend.</returns>
        public static ActionResult HandleError(
            ControllerBase endpoint,
            ILogger logger,
            IDeadLockDetector deadLock,
            Exception exception
            )
        {
            // Check validation exception.
            if (exception is ValidationException)
                //// Status code 422 = Unprocesssable Entity
                //return endpoint.UnprocessableEntity(
                //    new ValidationError(exception as ValidationException)
                //    );
                return endpoint.ValidationProblem();

            // Check deadlock exception.
            DeadlockException deadlock = deadLock.CheckException(exception);
            if (deadlock != null)
            {
                //// Status code 423 = Locked
                //result = new ObjectResult(deadlock);
                //result.StatusCode = StatusCodes.Status423Locked;
                //return result;
                return endpoint.Problem(deadlock.Message, null, StatusCodes.Status423Locked, "Transaction is deadlocked", null);
            }

            // Evaluate other exceptions.
            int statusCode;
            BackendError backend = BackendError.Evaluate(exception, out statusCode);

            logger.LogError(exception, backend.Summary, null);

            //result = new ObjectResult(backend);
            //result.StatusCode = statusCode;
            //return result;
            return endpoint.Problem(backend.Summary, null, statusCode, exception.Message, null);
        }
    }
}
