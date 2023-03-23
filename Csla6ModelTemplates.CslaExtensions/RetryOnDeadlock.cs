//using Csla6ModelTemplates.Dal;
//using Microsoft.AspNetCore.Mvc;

//namespace Csla6ModelTemplates.CslaExtensions
//{
//    internal static class RUN
//    {
//        internal const int MAX_RETRIES = 1;
//        internal const int MIN_DELAY_MS = 200;
//        internal const int MAX_DELAY_MS = 400;
//    }

//    /// <summary>
//    /// Provides method to execute a function and handle deadlock.
//    /// </summary>
//    public static class Run
//    {
//        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

//        /// <summary>
//        /// Executes a function, and retries when it fails due to deadlock.
//        /// </summary>
//        /// <param name="businessMethod">The function to execute.</param>
//        /// <param name="maxRetries">The number of attempts, defaults to 3.</param>
//        /// <returns>The result of the action.</returns>
//        public async static Task<ActionResult> RetryOnDeadlock(
//            Func<Task<ActionResult>> businessMethod,
//            int maxRetries = RUN.MAX_RETRIES
//            )
//        {
//            var retryCount = 0;
//            ActionResult result = null;

//            while (retryCount < maxRetries)
//            {
//                try
//                {
//                    result = await businessMethod();
//                }
//                catch (Exception)
//                {
//                    // Do nothing.
//                }

//                if ((result is OkObjectResult) &&
//                    ((OkObjectResult)result).Value is DeadlockException)
//                {
//                    retryCount++;
//                    result = null;
//                    Thread.Sleep(_random.Next(RUN.MIN_DELAY_MS, RUN.MAX_DELAY_MS));
//                }
//                else
//                    break;
//            }

//            return result;
//        }
//    }

//    /// <summary>
//    /// Provides method to execute a function and handle deadlock.
//    /// </summary>
//    /// <typeparam name="T">The type of the action result.</typeparam>
//    public static class Call<T> where T : class
//    {
//        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

//        /// <summary>
//        /// Executes a function, and retries when it fails due to deadlock.
//        /// </summary>
//        /// <param name="businessMethod">The function to execute.</param>
//        /// <param name="maxRetries">The number of attempts, defaults to 3.</param>
//        /// <returns>The result of the action.</returns>
//        public async static Task<ActionResult<T>> RetryOnDeadlock(
//            Func<Task<ActionResult<T>>> businessMethod,
//            int maxRetries = RUN.MAX_RETRIES
//            )
//        {
//            var retryCount = 0;
//            ActionResult<T> result = null;
//            Exception exception = null;

//            while (retryCount < maxRetries)
//            {
//                try
//                {
//                    result = await businessMethod();
//                }
//                catch (Exception ex)
//                {
//                    exception = ex;
//                }

//                if (exception is DeadlockException /*
//                    (result.Result is ObjectResult) &&
//                    ((ObjectResult)result.Result).Value is DeadlockException */
//                    )
//                {
//                    retryCount++;
//                    result = null;
//                    Thread.Sleep(_random.Next(RUN.MIN_DELAY_MS, RUN.MAX_DELAY_MS));
//                }
//                else
//                    break;
//            }

//            return result;
//        }
//    }

//    /// <summary>
//    /// Provides method to execute a function and handle deadlock.
//    /// </summary>
//    /// <typeparam name="T">The type of the action result.</typeparam>
//    public static class Call2<T> where T : class
//    {
//        private static readonly Random _random = new(DateTime.Now.Millisecond);

//        /// <summary>
//        /// Executes a function, and retries when it fails due to deadlock.
//        /// </summary>
//        /// <param name="businessMethod">The function to execute.</param>
//        /// <param name="maxRetries">The number of attempts, defaults to 3.</param>
//        /// <returns>The result of the action.</returns>
//        public async static Task<T> RetryOnDeadlock(
//            Func<Task<T>> businessMethod,
//            int maxRetries = RUN.MAX_RETRIES
//            )
//        {
//            var retryCount = 0;
//            while (true)
//            {
//                try
//                {
//                    return await businessMethod();
//                }
//                catch (Exception ex)
//                {
//                    retryCount++;
//                    if (ex is DeadlockException && retryCount <= maxRetries)
//                    {
//                        Thread.Sleep(_random.Next(RUN.MIN_DELAY_MS, RUN.MAX_DELAY_MS));
//                    }
//                    else
//                        throw;
//                }
//            }
//        }
//    }
//}
