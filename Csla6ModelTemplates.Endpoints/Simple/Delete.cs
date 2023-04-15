using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Models.Simple.Edit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Simple
{
    /// <summary>
    /// Deletes the specified team.
    /// </summary>
    [Route(Routes.Simple)]
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
        /// Deletes the specified team.
        /// </summary>
        /// <param name="id">The identifier of the team.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Deletes the specified team.",
            Description = "Deletes the specified team.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;teamId: string<br>" +
                "}",
            OperationId = "SimpleTeam.Delete",
            Tags = new[] { "Simple" })
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
                    await SimpleTeam.Delete(Csla.Factory, id);
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
