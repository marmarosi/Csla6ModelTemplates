using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Models.Complex.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Updates the specified team.
    /// </summary>
    [Route(Routes.Complex)]
    public class Update : EndpointBaseAsync
        .WithRequest<TeamDto>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Update(
            ILogger<Update> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Updates the specified team.
        /// </summary>
        /// <param name="dto">The data transer object of the team.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated team.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Updates the specified team.",
            Description = "Updates the specified team.<br>" +
                "Data: TeamDto<br>" +
                "Result: TeamDto",
            OperationId = "Team.Update",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] TeamDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return Ok(await Helper.RetryOnDeadlock(async () =>
                {
                    var team = await Team.Build(Csla.Factory, Csla.ChildFactory, dto);
                    if (team.IsSavable)
                    {
                        team = await team.SaveAsync();
                    }
                    return team.ToDto();
                }));
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
