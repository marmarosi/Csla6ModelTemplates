using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Simple.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Updates the specified team.
    /// </summary>
    [Route(Routes.Simple)]
    public class Update : EndpointBaseAsync
        .WithRequest<SimpleTeamDto>
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
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Updates the specified team.",
            Description = "Updates the specified team.<br>" +
                "Data: SimpleTeamDto<br>" +
                "Result: SimpleTeamDto",
            OperationId = "SimpleTeam.Update",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] SimpleTeamDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return Ok(await Helper.RetryOnDeadlock(async () =>
                {
                    var team = await SimpleTeam.Build(Csla.Factory, Csla.ChildFactory, dto);
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
