using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Selection.WithId;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Csla6ModelTemplates.Models.Selection.WithId;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Selection
{
    /// <summary>
    /// Gets the ID-name choice of the teams.
    /// </summary>
    [Route(Routes.Selection)]
    public class WithId : EndpointBaseAsync
        .WithRequest<TeamIdChoiceCriteria>
        .WithActionResult<IList<IdNameOptionDto>>
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public WithId(
            ILogger<WithId> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets the ID-name choice of the teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team choice.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The ID-name choice of the teams.</returns>
        [HttpGet("with-id")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets the ID-name choice of the teams.",
            Description = "Gets the ID-name choice of the teams.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string<br>" +
                "}<br>" +
                "Result: IdNameOptionDto[]",
            OperationId = "TeamIdChoice.List",
            Tags = new[] { "Selection" })
        ]
        public override async Task<ActionResult<IList<IdNameOptionDto>>> HandleAsync(
            [FromQuery] TeamIdChoiceCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var choice = await TeamIdChoice.Get(Csla.Factory, criteria);
                return Ok(choice.ToDto<IdNameOptionDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
