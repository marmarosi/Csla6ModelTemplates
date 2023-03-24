using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Selection.WithKey;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Models.Selection.WithKey;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Selection
{
    /// <summary>
    /// Gets the key-name choice of the teams.
    /// </summary>
    [Route(Routes.Selection)]
    public class WithKey : EndpointBaseAsync
        .WithRequest<TeamKeyChoiceCriteria>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public WithKey(
            ILogger<WithKey> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets the key-name choice of the teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team choice.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The key-name choice of the teams.</returns>
        [HttpGet("with-key")]
        [ProducesResponseType(typeof(List<KeyNameOptionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Gets the key-name choice of the teams.",
            Description = "Gets the key-name choice of the teams.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string<br>" +
                "}<br>" +
                "Result: KeyNameOptionDto[]",
            OperationId = "TeamKeyChoice.List",
            Tags = new[] { "Selection" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromQuery] TeamKeyChoiceCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var choice = await TeamKeyChoice.Get(Csla.Factory, criteria);
                return Ok(choice.ToDto<KeyNameOptionDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
