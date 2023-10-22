using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Junction.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Junction
{
    /// <summary>
    /// Updates the specified group.
    /// </summary>
    [Route(Routes.Junction)]
    public class Update : EndpointBaseAsync
        .WithRequest<GroupDto>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Update(
            ILogger<Update> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Updates the specified group.
        /// </summary>
        /// <param name="dto">The data transer object of the group.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated group.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Updates the specified group.",
            Description = "Updates the specified group.<br>" +
                "Data: GroupDto<br>" +
                "Result: GroupDto",
            OperationId = "Group.Update",
            Tags = new[] { "Junction" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] GroupDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return Ok(await Helper.RetryOnDeadlock(async () =>
                {
                    Group group = await Group.Build(Csla.Factory, Csla.ChildFactory, dto);
                    if (group.IsSavable)
                    {
                        group = await group.SaveAsync();
                    }
                    return group.ToDto();
                }));
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
