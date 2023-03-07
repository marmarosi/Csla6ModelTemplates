using Csla6ModelTemplates.CslaExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Csla6ModelTemplates.WebApiTests
{
    internal static class RUN
    {
        internal const int MAX_RETRIES = 2;
        internal const int MIN_DELAY_MS = 100;
        internal const int MAX_DELAY_MS = 200;
    }

    public static class Run
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public async static Task<ActionResult> RetryOnDeadlock(
            Func<Task<ActionResult>> businessMethod,
            int maxRetries = RUN.MAX_RETRIES
            )
        {
            var retryCount = 0;
            ActionResult result = null;

            while (retryCount < maxRetries)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    result = await businessMethod();
                    scope.Dispose();
                }

                if (result is OkObjectResult &&
                    (result as OkObjectResult).Value is DeadlockError)
                {
                    Console.Beep(170, 1500);
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

    public static class Call<T> where T : class
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        public async static Task<ActionResult<T>> RetryOnDeadlock(
            Func<Task<ActionResult<T>>> businessMethod,
            int maxRetries = RUN.MAX_RETRIES
            )
        {
            var retryCount = 0;
            ActionResult<T> result = null;

            while (retryCount < maxRetries)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    result = await businessMethod();
                }

                if (result.Result is ObjectResult &&
                    (result.Result as ObjectResult).Value is DeadlockError)
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
