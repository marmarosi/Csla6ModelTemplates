using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Provides helper methods for integration tests.
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// Tests whether the endpoint request threw a deadlock exception.
        /// If yes, then skip the assertions.
        /// </summary>
        /// <param name="actionResult">The result of an endpoint request.</param>
        /// <param name="testName">The name of the tests.</param>
        /// <returns>True when the endpoint crasheb by database deadlock; otherwise false.</returns>
        public bool IsDeadlock(
            object actionResult,
            string testName
            )
        {
            if (actionResult is ObjectResult objectResult &&
                objectResult is not OkObjectResult &&
                objectResult is not CreatedResult)
            {
                var problemDetails = objectResult.Value as ProblemDetails;
                Console.WriteLine("========== >>> " + testName);
                Console.WriteLine("           >>> " + problemDetails.Title);
                Console.WriteLine("           >>> " + problemDetails.Detail);
                if (objectResult.StatusCode == StatusCodes.Status423Locked)
                    return true;
            }
            return false;
        }
    }
}
