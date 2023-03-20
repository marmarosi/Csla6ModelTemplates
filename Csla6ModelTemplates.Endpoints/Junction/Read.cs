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
    /// Gets the specified group to edit.
    /// </summary>
    [Route(Routes.Junction)]
    public class Read : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult<GroupDto>
    {
        internal ILogger Logger { get; private set; }
        internal IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        public Read(
            ILogger<Read> logger,
            IDataPortalFactory factory
            )
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Gets the specified group to edit.
        /// </summary>
        /// <param name="id">The identifier of the group.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested group.</returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets the specified group to edit.",
            Description = "Gets the specified group to edit.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;groupId: string<br>" +
                "}<br>" +
                "Result: GroupDto",
            OperationId = "Group.Read",
            Tags = new[] { "Junction" })
        ]
        public override async Task<ActionResult<GroupDto>> HandleAsync(
            string id,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                Group group = await Group.Get(Factory, id);
                return Ok(group.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, ex);
            }
        }
    }
}
