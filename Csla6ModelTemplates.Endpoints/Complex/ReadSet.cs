using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Complex.Set;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Gets the specified team set to edit.
    /// </summary>
    [Route(Routes.Complex)]
    public class ReadSet : EndpointBaseAsync
        .WithRequest<TeamSetCriteria>
        .WithActionResult
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
        [ProducesResponseType(typeof(IList<TeamSetItemDto>), StatusCodes.Status200OK)]
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
        public override async Task<ActionResult> HandleAsync(
            [FromQuery] TeamSetCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var teams = await TeamSet.Get(Csla.Factory, criteria);
                return Ok(teams.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
