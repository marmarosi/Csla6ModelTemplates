using Csla6ModelTemplates.CslaExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi
{
    internal static class RUN
    {
        internal const int MAX_RETRIES = 1;
        internal const int MIN_DELAY_MS = 200;
        internal const int MAX_DELAY_MS = 400;
    }

    /// <summary>
    /// Provides method to execute a function and handle deadlock.
    /// </summary>
    public static class Run
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Executes a function, and retries when it fails due to deadlock.
        /// </summary>
        /// <param name="businessMethod">The function to execute.</param>
        /// <param name="maxRetries">The number of attempts, defaults to 3.</param>
        /// <returns>The result of the action.</returns>
        public async static Task<ActionResult> RetryOnDeadlock(
            Func<Task<ActionResult>> businessMethod,
            int maxRetries = RUN.MAX_RETRIES
            )
        {
            var retryCount = 0;
            ActionResult result = null;

            while (retryCount < maxRetries)
            {
                result = await businessMethod();

                if ((result is OkObjectResult) &&
                    ((OkObjectResult)result).Value is DeadlockError)
                {
                    retryCount++;
                    result = null;
                    Thread.Sleep(_random.Next(RUN.MIN_DELAY_MS, RUN.MAX_DELAY_MS));
                }
                else
                    break;
            }

            return result;
        }
    }

    /// <summary>
    /// Provides method to execute a function and handle deadlock.
    /// </summary>
    /// <typeparam name="T">The type of the action result.</typeparam>
    public static class Call<T> where T : class
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Executes a function, and retries when it fails due to deadlock.
        /// </summary>
        /// <param name="businessMethod">The function to execute.</param>
        /// <param name="maxRetries">The number of attempts, defaults to 3.</param>
        /// <returns>The result of the action.</returns>
        public async static Task<ActionResult<T>> RetryOnDeadlock(
            Func<Task<ActionResult<T>>> businessMethod,
            int maxRetries = RUN.MAX_RETRIES
            )
        {
            var retryCount = 0;
            ActionResult<T> result = null;

            while (retryCount < maxRetries)
            {
                result = await businessMethod();

                if ((result.Result is ObjectResult) &&
                    ((ObjectResult)result.Result).Value is DeadlockError)
                {
                    retryCount++;
                    result = null;
                    Thread.Sleep(_random.Next(RUN.MIN_DELAY_MS, RUN.MAX_DELAY_MS));
                }
                else
                    break;
            }

            return result;
        }
    }
}
