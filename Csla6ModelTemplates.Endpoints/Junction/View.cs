using Ardalis.ApiEndpoints;
using Csla;
using Csla6ModelTemplates.Contracts.Junction.View;
using Csla6ModelTemplates.Models.Junction.View;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Junction
{
    /// <summary>
    /// Gets the specified group details to display.
    /// </summary>
    [Route(Routes.Junction)]
    public class View : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult<GroupViewDto>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public View(
            ILogger<View> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets the specified group details to display.
        /// </summary>
        /// <param name="id">The identifier of the group.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested group view.</returns>
        [HttpGet("{id}/view")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets the specified group details to display.",
            Description = "Gets the specified group details to display.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;groupId: string<br>" +
                "}<br>" +
                "Result: GroupViewDto",
            OperationId = "Group.View",
            Tags = new[] { "Junction" })
        ]
        public override async Task<ActionResult<GroupViewDto>> HandleAsync(
            string id,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                GroupView group = await GroupView.Get(Factory, id);
                return Ok(group.ToDto<GroupViewDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
