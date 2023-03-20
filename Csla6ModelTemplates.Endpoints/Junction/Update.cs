using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.Models.Junction.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Junction
{
    /// <summary>
    /// Updates the specified group.
    /// </summary>
    [Route(Routes.Junction)]
    public class Update : EndpointBaseAsync
        .WithRequest<GroupDto>
        .WithActionResult<GroupDto>
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
        public Update(
            ILogger<Update> logger,
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory
            )
        {
            Logger = logger;
            Factory = factory;
            ChildFactory = childFactory;
        }

        /// <summary>
        /// Updates the specified group.
        /// </summary>
        /// <param name="dto">The data transer object of the group.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated group.</returns>
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Updates the specified group.",
            Description = "Updates the specified group.<br>" +
                "Data: GroupDto<br>" +
                "Result: GroupDto",
            OperationId = "Group.Update",
            Tags = new[] { "Junction" })
        ]
        public override async Task<ActionResult<GroupDto>> HandleAsync(
            [FromBody] GroupDto dto,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                return await Call<GroupDto>.RetryOnDeadlock(async () =>
                {
                    Group group = await Group.Build(Factory, ChildFactory, dto);
                    if (group.IsSavable)
                    {
                        group = await group.SaveAsync();
                    }
                    return Ok(group.ToDto());
                });
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
