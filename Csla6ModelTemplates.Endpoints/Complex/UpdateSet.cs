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
    /// Updates the specified team set.
    /// </summary>
    [Route(Routes.Complex)]
    public class UpdateSet : EndpointBaseAsync
        .WithRequest<TeamSetRequest>
        .WithActionResult<IList<TeamSetItemDto>>
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
        public UpdateSet(
            ILogger<UpdateSet> logger,
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory
            )
        {
            Logger = logger;
            Factory = factory;
            ChildFactory = childFactory;
        }

        /// <summary>
        /// Updates the specified team set.
        /// </summary>
        /// <param name="request">
        ///     criteria:   The criteria of the team set.
        ///     dto:        The data transer objects of the team set.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated team.</returns>
        [HttpPut("set")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Updates the specified team set.",
            Description = "Updates the specified team set<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string<br>" +
                "}<br>" +
                "Data: TeamSetItemDto[]<br>" +
                "Result: TeamSetItemDto[]",
            OperationId = "TeamSet.Update",
            Tags = new[] { "Complex" })
        ]
        public override async Task<ActionResult<IList<TeamSetItemDto>>> HandleAsync(
            [FromRoute] TeamSetRequest request,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return await Call<IList<TeamSetItemDto>>.RetryOnDeadlock(async () =>
                {
                    var teams = await TeamSet.Build(Factory, ChildFactory, request.Criteria, request.Dto);
                    if (teams.IsSavable)
                    {
                        teams = await teams.SaveAsync();
                    }
                    return Ok(teams.ToDto());
                });
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }

    /// <summary>
    /// Defines the input data to update a team set.
    /// </summary>
    public class TeamSetRequest
    {
        /// <summary>
        /// The criteria of the team set.
        /// </summary>
        [FromQuery] public TeamSetCriteria Criteria { get; set; }

        /// <summary>
        /// The data transer objects of the team set.
        /// </summary>
        [FromBody] public List<TeamSetItemDto> Dto { get; set; }
    }
}
