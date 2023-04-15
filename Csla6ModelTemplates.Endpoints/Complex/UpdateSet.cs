using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Models.Complex.Set;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Complex
{
    /// <summary>
    /// Updates the specified team set.
    /// </summary>
    [Route(Routes.Complex)]
    public class UpdateSet : EndpointBaseAsync
        .WithRequest<TeamSetRequest>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public UpdateSet(
            ILogger<UpdateSet> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
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
        [ProducesResponseType(typeof(IList<TeamSetItemDto>), StatusCodes.Status200OK)]
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
        public override async Task<ActionResult> HandleAsync(
            [FromRoute] TeamSetRequest request,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return Ok(await Helper.RetryOnDeadlock(async () =>
                {
                    var teams = await TeamSet.Build(Csla.Factory, Csla.ChildFactory, request.Criteria, request.Dto);
                    if (teams.IsSavable)
                    {
                        teams = await teams.SaveAsync();
                    }
                    return teams.ToDto();
                }));
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
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
        public TeamSetCriteria Criteria { get; set; }

        /// <summary>
        /// The data transer objects of the team set.
        /// </summary>
        public List<TeamSetItemDto> Dto { get; set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="criteria">The criteria of the team set.</param>
        /// <param name="dto">The data transer objects of the team set.</param>
        public TeamSetRequest(
            TeamSetCriteria criteria,
            List<TeamSetItemDto> dto
            )
        {
            Criteria = criteria;
            Dto = dto;
        }
    }
}
