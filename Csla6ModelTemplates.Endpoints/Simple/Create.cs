using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Models.Simple.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Creates a new team.
    /// </summary>
    [Route(Routes.Simple)]
    public class Create : EndpointBaseAsync
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
        public Create(
            ILogger<Create> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="dto">The data transer object of the team.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created team.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status201Created)]
        [SwaggerOperation(
            Summary = "Creates a new team.",
            Description = "Creates a new team.<br>" +
                "Data: SimpleTeamDto<br>" +
                "Result: SimpleTeamDto",
            OperationId = "SimpleTeam.Create",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] SimpleTeamDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return Created(Helper.Uri(Request), await Helper.RetryOnDeadlock(async () =>
                {
                    var team = await SimpleTeam.Build(Csla.Factory, Csla.ChildFactory, dto);
                    if (team.IsValid)
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
