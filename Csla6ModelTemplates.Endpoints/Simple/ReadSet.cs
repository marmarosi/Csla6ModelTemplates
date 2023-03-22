using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Csla6ModelTemplates.Models.Simple.Set;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Gets the specified team set to edit.
    /// </summary>
    [Route(Routes.Simple)]
    public class ReadSet : EndpointBaseAsync
        .WithRequest<SimpleTeamSetCriteria>
        .WithActionResult<IList<SimpleTeamSetItemDto>>
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public ReadSet(
            ILogger<ReadSet> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
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
                "Result: SimpleTeamSetItemDto[]",
            OperationId = "SimpleTeamSet.Read",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult<IList<SimpleTeamSetItemDto>>> HandleAsync(
            [FromQuery] SimpleTeamSetCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var teams = await SimpleTeamSet.Get(Csla.Factory, criteria);
                return Ok(teams.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
