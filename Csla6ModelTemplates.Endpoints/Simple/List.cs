using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.Models.Simple.List;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Gets a list of teams.
    /// </summary>
    [Route(Routes.Simple)]
    public class List : EndpointBaseAsync
        .WithRequest<SimpleTeamListCriteria>
        .WithActionResult<IList<SimpleTeamListItemDto>>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public List(
            ILogger<List> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets a list of teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of teams.</returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets a list of teams.",
            Description = "Gets a list of teams.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;TeamName: string<br>" +
                "}<br>" +
                "Result: SimpleTeamListItemDto[]",
            OperationId = "SimpleTeam.List",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult<IList<SimpleTeamListItemDto>>> HandleAsync(
            [FromQuery] SimpleTeamListCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var teams = await SimpleTeamList.Get(Factory, criteria);
                return Ok(teams.ToDto<SimpleTeamListItemDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
