using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Arrangement.Sorting;
using Csla6ModelTemplates.Models.Arrangement.Sorting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Arrangement
{
    /// <summary>
    /// Gets the specified teams sorted.
    /// </summary>
    [Route(Routes.Arrangement)]
    public class Sorting : EndpointBaseAsync
        .WithRequest<SortedTeamListCriteria>
        .WithActionResult<IList<SortedTeamListItemDto>>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public Sorting(
            ILogger<Sorting> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets the specified teams sorted.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested team list.</returns>
        [HttpGet("sorted")]
        [Produces(MediaTypeNames.Application.Json)]
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
        public override async Task<ActionResult<IList<SortedTeamListItemDto>>> HandleAsync(
            [FromQuery] SortedTeamListCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                SortedTeamList list = await SortedTeamList.Get(Factory, criteria);
                return Ok(list.ToDto<SortedTeamListItemDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
