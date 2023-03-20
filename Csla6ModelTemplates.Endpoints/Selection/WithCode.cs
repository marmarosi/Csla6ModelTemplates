using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Selection.WithCode;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Models.Selection.WithCode;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Selection
{
    /// <summary>
    /// Gets the code-name choice of the teams.
    /// </summary>
    [Route(Routes.Selection)]
    public class WithCode : EndpointBaseAsync
        .WithRequest<TeamCodeChoiceCriteria>
        .WithActionResult<IList<CodeNameOptionDto>>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public WithCode(
            ILogger<WithCode> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets the code-name choice of the teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team choice.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The key-name choice of the teams.</returns>
        [HttpGet("with-code")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets the code-name choice of the teams.",
            Description = "Gets the code-name choice of the teams.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string<br>" +
                "}<br>" +
                "Result: CodeNameOptionDto[]",
            OperationId = "ChoiceWithCode.List",
            Tags = new[] { "Selection" })
        ]
        public override async Task<ActionResult<IList<CodeNameOptionDto>>> HandleAsync(
            [FromQuery] TeamCodeChoiceCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var choice = await TeamCodeChoice.Get(Factory, criteria);
                return Ok(choice.ToDto<CodeNameOptionDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
