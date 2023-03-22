using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Csla6ModelTemplates.Models.Simple.Set;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Updates the specified team set.
    /// </summary>
    [Route(Routes.Simple)]
    public class UpdateSet : EndpointBaseAsync
        .WithRequest<SimpleTeamSetRequest>
        .WithActionResult<IList<SimpleTeamSetItemDto>>
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
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Updates the specified team set.",
            Description = "Updates the specified team set<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamName: string<br>" +
                "}<br>" +
                "Data: SimpleTeamSetItemDto[]<br>" +
                "Result: SimpleTeamSetItemDto[]",
            OperationId = "SimpleTeamSet.Update",
            Tags = new[] { "Simple" })
        ]
        public override async Task<ActionResult<IList<SimpleTeamSetItemDto>>> HandleAsync(
            [FromRoute] SimpleTeamSetRequest request,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return await Call<IList<SimpleTeamSetItemDto>>.RetryOnDeadlock(async () =>
                {
                    var teams = await SimpleTeamSet.Build(Csla.Factory, Csla.ChildFactory, request.Criteria, request.Dto);
                    if (teams.IsSavable)
                    {
                        teams = await teams.SaveAsync();
                    }
                    return Ok(teams.ToDto());
                });
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
    public class SimpleTeamSetRequest
    {
        /// <summary>
        /// The criteria of the team set.
        /// </summary>
        [FromQuery] public SimpleTeamSetCriteria Criteria { get; set; }

        /// <summary>
        /// The data transer objects of the team set.
        /// </summary>
        [FromBody] public List<SimpleTeamSetItemDto> Dto { get; set; }
    }
}