using Csla6ModelTemplates.CslaExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Csla6ModelTemplates.EndpointTests
{
    internal static class Wait
    {
        private static readonly Random RandomInt = new(DateTime.Now.Millisecond);

        internal const int MAX_RETRIES = 2;
        internal const int MIN_DELAY_MS = 100;
        internal const int MAX_DELAY_MS = 200;

        internal static int RandomTime()
        {
            return RandomInt.Next(Wait.MIN_DELAY_MS, Wait.MAX_DELAY_MS);
        }
    }

    public static class Run
    {
        /// <summary>
        /// Executes a business method, and retries to run it again
        /// when it fails with dead lock error.
        /// </summary>
        /// <param name="businessMethod">The business method to run.</param>
        /// <param name="maxRetries">The number of trials, the default value is 2.</param>
        /// <returns>The result of the business method or an exception.</returns>
        public async static Task<ActionResult> RetryOnDeadlock(
            Func<Task<ActionResult>> businessMethod,
            int maxRetries = Wait.MAX_RETRIES
            )
        {
            var retryCount = 0;
            ActionResult result = null;

            while (retryCount < maxRetries)
            {
                result = await businessMethod();

                if (result is OkObjectResult &&
                   (result as OkObjectResult).Value is DeadlockError)
                {
                    retryCount++;
                    result = null;
                    Thread.Sleep(Wait.RandomTime());
                }
                else
                    break;
            }

            return result;
        }
    }

    public static class Call<T> where T : class
    {
        /// <summary>
        /// Executes a business method, and retries to run it again
        /// when it fails with dead lock error.
        /// </summary>
        /// <param name="businessMethod">The business method to run.</param>
        /// <param name="maxRetries">The number of trials, the default value is 2.</param>
        /// <returns>The result of the business method or an exception.</returns>
        public async static Task<ActionResult<T>> RetryOnDeadlock(
            Func<Task<ActionResult<T>>> businessMethod,
            int maxRetries = Wait.MAX_RETRIES
            )
        {
            var retryCount = 0;
            ActionResult<T> result = null;

            while (retryCount < maxRetries)
            {
                result = await businessMethod();

                if (result.Result is ObjectResult &&
                   (result.Result as ObjectResult).Value is DeadlockError)
                {
                    retryCount++;
                    result = null;
                    Thread.Sleep(Wait.RandomTime());
                }
                else
                    break;
            }

            return result;
        }
    }
}
