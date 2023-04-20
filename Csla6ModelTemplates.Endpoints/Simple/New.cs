using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Simple.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Gets a new team to edit.
    /// </summary>
    [Route(Routes.Simple)]
    public class New : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public New(
            ILogger<New> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets a new team to edit.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new team..</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Gets a new team to edit.",
            Description = "Gets a new team to edit.<br>" +
                "Result: SimpleTeamDto",
            OperationId = "SimpleTeam.New",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult> HandleAsync(
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var team = await SimpleTeam.New(Csla.Factory);
                return Ok(team.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
