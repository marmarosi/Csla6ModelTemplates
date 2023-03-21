using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Complex.View;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Csla6ModelTemplates.Models.Complex.View;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Gets the specified team details to display.
    /// </summary>
    [Route(Routes.Complex)]
    public class View : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult<TeamViewDto>
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public View(
            ILogger<Full> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets the specified team details to display.
        /// </summary>
        /// <param name="id">The identifier of the team.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested team view.</returns>
        [HttpGet("{id}/view")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets the specified team details to display.",
            Description = "Gets the specified team details to display.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamId: string<br>" +
                "}<br>" +
                "Result: TeamViewDto",
            OperationId = "Team.View",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult<TeamViewDto>> HandleAsync(
            string id,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var team = await TeamView.Get(Csla.Factory, id);
                return Ok(team.ToDto<TeamViewDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
