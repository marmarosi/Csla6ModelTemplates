using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Arrangement.Sorting;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Arrangement.Sorting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Arrangement
{
    /// <summary>
    /// Gets the specified teams sorted.
    /// </summary>
    [Route(Routes.Arrangement)]
    public class Sorting : EndpointBaseAsync
        .WithRequest<SortedTeamListCriteria>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Sorting(
            ILogger<Sorting> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets the specified teams sorted.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested team list.</returns>
        [HttpGet("sorted")]
        [ProducesResponseType(typeof(List<SortedTeamListItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Gets the specified teams sorted.",
            Description = "Gets the specified teams sorted.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;sortBy: string,<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;sortDirection: ascending | descending<br>" +
                "}<br>" +
                "Result: SortedTeamListItemDto[]",
            OperationId = "SortedTeam.List",
            Tags = new[] { "Arrangement" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromQuery] SortedTeamListCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                SortedTeamList list = await SortedTeamList.Get(Csla.Factory, criteria);
                return Ok(list.ToDto<SortedTeamListItemDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
