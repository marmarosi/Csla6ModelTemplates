using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Models.Junction.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Junction
{
    /// <summary>
    /// Gets a new group to edit.
    /// </summary>
    [Route(Routes.Junction)]
    public class New : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public New(
            ILogger<New> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets a new group to edit.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new group.</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Gets a new group to edit.",
            Description = "Gets a new group to edit.<br>" +
                "Result: GroupDto",
            OperationId = "Group.New",
            Tags = new[] { "Junction" })
        ]
        public override async Task<ActionResult> HandleAsync(
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                Group group = await Group.New(Csla.Factory);
                return Ok(group.ToDto());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
