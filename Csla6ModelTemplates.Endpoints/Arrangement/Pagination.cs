using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Arrangement.Pagination;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Models.Arrangement.Pagination;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Arrangement
{
    /// <summary>
    /// Gets the specified page of teams.
    /// </summary>
    [Route(Routes.Arrangement)]
    public class Pagination : EndpointBaseAsync
        .WithRequest<PaginatedTeamListCriteria>
        .WithActionResult<IPaginatedList<PaginatedTeamListItemDto>>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public Pagination(
            ILogger<Pagination> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets the specified page of teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested page of the team list.</returns>
        [HttpGet("paginated")]
        [Produces(MediaTypeNames.Application.Json)]
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
        public override async Task<ActionResult<IPaginatedList<PaginatedTeamListItemDto>>> HandleAsync(
            [FromQuery] PaginatedTeamListCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                PaginatedTeamList list = await PaginatedTeamList.Get(Factory, criteria);
                return Ok(list.ToPaginatedDto<PaginatedTeamListItemDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
