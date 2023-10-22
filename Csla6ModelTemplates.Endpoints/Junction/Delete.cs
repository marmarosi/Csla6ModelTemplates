using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Junction.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Junction
{
    /// <summary>
    /// Deletes the specified group.
    /// </summary>
    [Route(Routes.Junction)]
    public class Delete : EndpointBaseAsync
        .WithRequest<string>
        .WithoutResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Delete(
            ILogger<Delete> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Deletes the specified group.
        /// </summary>
        /// <param name="id">The identifier of the group.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Deletes the specified group.",
            Description = "Deletes the specified group.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;groupId: string<br>" +
                "}",
            OperationId = "Group.Delete",
            Tags = new[] { "Junction" })
        ]
        public override async Task<ActionResult> HandleAsync(
            string id,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                await Helper.RetryOnDeadlock(async () =>
                {
                    await Task.Run(() => Group.Delete(Csla.Factory, id));
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
