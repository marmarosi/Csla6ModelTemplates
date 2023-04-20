using Csla;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.CslaExtensions.Validations;
using Csla6ModelTemplates.Dal;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi
{
    /// <summary>
    /// Serves as the base class for the API controllers.
    /// </summary>
    public class ApiController : ControllerBase
    {
        #region Properties

        private const int MAX_RETRIES = 1;
        private const int MIN_DELAY_MS = 500;
        private const int MAX_DELAY_MS = 1000;

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

        #endregion

        #region Constructor

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

        #endregion

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

        #region HandleError

        /// <summary>
        /// Handles the eventual exceptions.
        /// </summary>
        /// <param name="exception">The exception thrown by the backend.</param>
        /// <returns>The error information to send to the frontend.</returns>
        protected IActionResult HandleError(
            Exception exception
            )
        {
            // Check validation exception.
            if (exception is ValidationException validation)
            {
                var messages = validation.Messages
                    .Select(m => new
                    {
                        Property = $"{m.Model}.{m.Property}",
                        m.Description
                    })
                    .GroupBy(
                        o => o.Property,
                        (key, grp) => new
                        {
                            Property = key,
                            Descriptions = grp.Select(g => g.Description).ToArray()
                        }
                    )
                    .ToDictionary(o => o.Property, o => o.Descriptions);

                var descriptor = new ValidationProblemDetails(messages);
                return ValidationProblem(descriptor);
            }

            // Check deadlock exception.
            DeadlockException deadlock = DeadLock.CheckException(exception);
            if (deadlock != null)
                return Problem(
                    deadlock.Message,
                    null,
                    StatusCodes.Status423Locked,
                    "Transaction is deadlocked",
                    null
                    );

            // Evaluate other exceptions.
            int statusCode;
            BackendError backend = BackendError.Evaluate(exception, out statusCode);

            Logger.LogError(exception, backend.Summary, null);

            //result = new ObjectResult(backend);
            //result.StatusCode = statusCode;
            //return result;
            return Problem(backend.Summary, null, statusCode, exception.Message, null);
        }

        #endregion
    }
}
