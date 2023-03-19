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
    /// Creates a new team.
    /// </summary>
    [Route(Routes.Complex)]
    public class Create : EndpointBaseAsync
        .WithRequest<TeamDto>
        .WithActionResult<TeamDto>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }
        internal IChildDataPortalFactory ChildFactory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        public Create(
            ILogger<Create> logger,
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory
            )
        {
            Logger = logger;
            Factory = factory;
            ChildFactory = childFactory;
        }

        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="dto">The data transer object of the team.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created team.</returns>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerOperation(
            Summary = "Creates a new team.",
            Description = "Creates a new team.<br>" +
                "Data: TeamDto<br>" +
                "Result: TeamDto",
            OperationId = "Team.Create",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult<TeamDto>> HandleAsync(
            [FromBody] TeamDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return await Call<TeamDto>.RetryOnDeadlock(async () =>
                {
                    var team = await Team.Build(Factory, ChildFactory, dto);
                    if (team.IsValid)
                    {
                        team = await team.SaveAsync();
                    }
                    return Created(Helper.Uri(Request), team.ToDto());
                });
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
