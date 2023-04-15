using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Arrangement.Full;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Models.Arrangement.Full;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Arrangement
{
    /// <summary>
    /// Gets the specified page of sorted teams.
    /// </summary>
    [Route(Routes.Arrangement)]
    public class Arranged : EndpointBaseAsync
        .WithRequest<ArrangedTeamListCriteria>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Arranged(
            ILogger<Arranged> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets the specified page of sorted teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested page of the sorted team list.</returns>
        [HttpGet("arranged")]
        [ProducesResponseType(typeof(PaginatedList<ArrangedTeamListItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Gets the specified page of sorted teams.",
            Description = "Gets the specified page of sorted teams.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;sortBy: string,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;sortDirection: ascending | descending,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;pageIndex: number,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;pageSize: number<br>" +
                "}<br>" +
                "Result: ArrangedTeamListItemDto[]",
            OperationId = "ArrangedTeam.List",
            Tags = new[] { "Arrangement" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromQuery] ArrangedTeamListCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var list = await ArrangedTeamList.Get(Csla.Factory, criteria);
                return Ok(list.ToPaginatedDto<ArrangedTeamListItemDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
