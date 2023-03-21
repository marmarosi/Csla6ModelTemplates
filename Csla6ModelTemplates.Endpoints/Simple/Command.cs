using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Csla6ModelTemplates.Models.Simple.Command;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Renames the specified team.
    /// </summary>
    [Route(Routes.Simple)]
    public class Command : EndpointBaseAsync
        .WithRequest<RenameTeamDto>
        .WithActionResult<bool>
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Command(
            ILogger<Full> logger,
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
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Renames the specified team.",
            Description = "Renames the specified team.<br>" +
                "Data: RenameTeamDto<br>" +
                "Result: boolean",
            OperationId = "SimpleTeam.Rename",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult<bool>> HandleAsync(
            [FromBody] RenameTeamDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return await Run.RetryOnDeadlock(async () =>
                {
                    var command = await RenameTeam.Execute(Csla.Factory, dto);
                    return Ok(command.Result);
                });
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
