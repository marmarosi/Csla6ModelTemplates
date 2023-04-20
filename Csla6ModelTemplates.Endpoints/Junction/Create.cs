using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Junction.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Junction
{
    /// <summary>
    /// Creates a new group.
    /// </summary>
    [Route(Routes.Junction)]
    public class Create : EndpointBaseAsync
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
        public Create(
            ILogger<Create> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Creates a new group.
        /// </summary>
        /// <param name="dto">The data transer object of the group.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created group.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status201Created)]
        [SwaggerOperation(
            Summary = "Creates a new group.",
            Description = "Creates a new group.<br>" +
                "Data: GroupDto<br>" +
                "Result: GroupDto",
            OperationId = "Group.Create",
            Tags = new[] { "Junction" })
        ]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] GroupDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return Created(Helper.Uri(Request), await Helper.RetryOnDeadlock(async () =>
                {
                    Group group = await Group.Build(Csla.Factory, Csla.ChildFactory, dto);
                    if (group.IsValid)
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
