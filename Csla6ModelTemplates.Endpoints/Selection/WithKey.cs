using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Selection.WithKey;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Models.Selection.WithKey;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Selection
{
    /// <summary>
    /// Gets the key-name choice of the teams.
    /// </summary>
    [Route(Routes.Selection)]
    public class WithKey : EndpointBaseAsync
        .WithRequest<TeamKeyChoiceCriteria>
        .WithActionResult<IList<KeyNameOptionDto>>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public WithKey(
            ILogger<WithKey> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets the key-name choice of the teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team choice.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The key-name choice of the teams.</returns>
        [HttpGet("with-key")]
        [Produces(MediaTypeNames.Application.Json)]
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
        public override async Task<ActionResult<IList<KeyNameOptionDto>>> HandleAsync(
            [FromQuery] TeamKeyChoiceCriteria criteria,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var choice = await TeamKeyChoice.Get(Factory, criteria);
                return Ok(choice.ToDto<KeyNameOptionDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
