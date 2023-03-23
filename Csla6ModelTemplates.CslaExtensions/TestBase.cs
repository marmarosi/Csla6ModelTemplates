using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.CslaExtensions
{
    public class TestBase
    {
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
                if (objectResult.StatusCode == 422)
                    return true;
            }
            return false;
        }
    }
}
