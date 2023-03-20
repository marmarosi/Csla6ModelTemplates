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
    /// Gets a new group to edit.
    /// </summary>
    [Route(Routes.Junction)]
    public class New : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<GroupDto>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public New(
            ILogger<New> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets a new group to edit.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new group.</returns>
        [HttpGet("new")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets a new group to edit.",
            Description = "Gets a new group to edit.<br>" +
                "Result: GroupDto",
            OperationId = "Group.New",
            Tags = new[] { "Junction" })
        ]
        public override async Task<ActionResult<GroupDto>> HandleAsync(
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                Group group = await Group.New(Factory);
                return Ok(group.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
