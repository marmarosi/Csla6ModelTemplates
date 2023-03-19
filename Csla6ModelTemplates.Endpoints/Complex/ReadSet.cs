using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.Models.Complex.Set;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Gets the specified team set to edit.
    /// </summary>
    [Route(Routes.Complex)]
    public class ReadSet : EndpointBaseAsync
        .WithRequest<TeamSetCriteria>
        .WithActionResult<IList<TeamSetItemDto>>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public ReadSet(
            ILogger<ReadSet> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets the specified team set to edit.
        /// </summary>
        /// <param name="criteria">The criteria of the team set.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested team set.</returns>
        [HttpGet("set")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets the specified team set to edit.",
            Description = "Gets the specified team set to edit.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string<br>" +
                "}<br>" +
                "Result: TeamSetItemDto[]",
            OperationId = "TeamSet.Read",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult<IList<TeamSetItemDto>>> HandleAsync(
            [FromQuery] TeamSetCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var teams = await TeamSet.Get(Factory, criteria);
                return Ok(teams.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
