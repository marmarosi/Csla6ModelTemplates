using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Complex.Command;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Counts the teams grouped by the number of their items.
    /// </summary>
    [Route(Routes.Complex)]
    public class Command : EndpointBaseAsync
        .WithRequest<CountTeamsCriteria>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Command(
            ILogger<Command> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Counts the teams grouped by the number of their items.
        /// </summary>
        /// <param name="criteria">The criteria of the count teams by item count command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True when the team was renamed; otherwise false.</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(List<CountTeamsResultDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Counts the teams grouped by the number of their items.",
            Description = "Counts the teams grouped by the number of their items.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string<br>" +
                "}<br>" +
                "Result: CountTeamsListItemDto",
            OperationId = "SimpleTeam.Rename",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] CountTeamsCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var command = await CountTeams.Execute(Csla.Factory, criteria);
                return Ok(command.Results.ToDto<CountTeamsResultDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
