using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.Models.Complex.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Gets the specified team to edit.
    /// </summary>
    [Route(Routes.Complex)]
    public class Read : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult<TeamDto>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public Read(
            ILogger<Read> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets the specified team to edit.
        /// </summary>
        /// <param name="id">The identifier of the team.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested team.</returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets the specified team to edit.",
            Description = "Gets the specified team details to edit.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamId: string<br>" +
                "}<br>" +
                "Result: TeamDto",
            OperationId = "Team.Read",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult<TeamDto>> HandleAsync(
            string id,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var team = await Team.Get(Factory, id);
                return Ok(team.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
