using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Csla6ModelTemplates.Models.Complex.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Gets e new team to edit.
    /// </summary>
    [Route(Routes.Complex)]
    public class New : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<TeamDto>
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public New(
            ILogger<New> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets e new team to edit.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new team..</returns>
        [HttpGet("new")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets e new team to edit.",
            Description = "Gets e new team to edit.<br>" +
                "Result: TeamDto",
            OperationId = "Team.New",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult<TeamDto>> HandleAsync(
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var team = await Team.New(Csla.Factory);
                return Ok(team.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
