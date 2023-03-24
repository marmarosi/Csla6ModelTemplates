using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Models.Simple.Command;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Renames the specified team.
    /// </summary>
    [Route(Routes.Simple)]
    public class Command : EndpointBaseAsync
        .WithRequest<RenameTeamDto>
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
        /// Renames the specified team.
        /// </summary>
        /// <param name="dto">The data transer object of the rename team command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True when the team was renamed; otherwise false.</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Renames the specified team.",
            Description = "Renames the specified team.<br>" +
                "Data: RenameTeamDto<br>" +
                "Result: boolean",
            OperationId = "SimpleTeam.Rename",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] RenameTeamDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return Ok(await Helper.RetryOnDeadlock(async () =>
                {
                    var command = await RenameTeam.Execute(Csla.Factory, dto);
                    return command.Result;
                }));
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
