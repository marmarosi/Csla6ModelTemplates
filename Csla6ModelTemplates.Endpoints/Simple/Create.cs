using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.Models.Simple.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Creates a new team.
    /// </summary>
    [Route(Routes.Simple)]
    public class Create : EndpointBaseAsync
        .WithRequest<SimpleTeamDto>
        .WithActionResult<SimpleTeamDto>
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
                "Data: SimpleTeamDto<br>" +
                "Result: SimpleTeamDto",
            OperationId = "SimpleTeam.Create",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult<SimpleTeamDto>> HandleAsync(
            [FromBody] SimpleTeamDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return await Call<SimpleTeamDto>.RetryOnDeadlock(async () =>
                {
                    var team = await SimpleTeam.Build(Factory, ChildFactory, dto);
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
