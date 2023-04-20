using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Arrangement.Pagination;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Models.Arrangement.Pagination;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Arrangement
{
    /// <summary>
    /// Gets the specified page of teams.
    /// </summary>
    [Route(Routes.Arrangement)]
    public class Pagination : EndpointBaseAsync
        .WithRequest<PaginatedTeamListCriteria>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Pagination(
            ILogger<Pagination> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets the specified page of teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested page of the team list.</returns>
        [HttpGet("paginated")]
        [ProducesResponseType(typeof(PaginatedList<PaginatedTeamListItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Gets the specified page of teams.",
            Description = "Gets the specified page of teams.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;pageIndex: number,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;pageSize: number<br>" +
                "}<br>" +
                "Result: PaginatedTeamListItemDto[]",
            OperationId = "PaginatedTeam.List",
            Tags = new[] { "Arrangement" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromQuery] PaginatedTeamListCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                PaginatedTeamList list = await PaginatedTeamList.Get(Csla.Factory, criteria);
                return Ok(list.ToPaginatedDto<PaginatedTeamListItemDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
