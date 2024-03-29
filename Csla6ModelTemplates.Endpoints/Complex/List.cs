using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Complex.List;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Gets a list of teams.
    /// </summary>
    [Route(Routes.Complex)]
    public class List : EndpointBaseAsync
        .WithRequest<TeamListCriteria>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public List(
            ILogger<List> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets a list of teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of teams.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<TeamListItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Gets a list of teams.",
            Description = "Gets a list of teams.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string<br>" +
                "}<br>" +
                "Result: TeamListItemDto[]",
            OperationId = "Team.List",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromQuery] TeamListCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var teams = await TeamList.Get(Csla.Factory, criteria);
                return Ok(teams.ToDto<TeamListItemDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
