using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.Contracts.Tree.View;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Models.Tree.View;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Csla6ModelTemplates.Endpoints.Tree
{
    /// <summary>
    /// Gets the specified folder tree.
    /// </summary>
    [Route(Routes.Tree)]
    public class Tree : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Tree(
            ILogger<Tree> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets the specified folder tree.
        /// </summary>
        /// <param name="id">The identifier of the root folder.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested folder tree.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<FolderNodeDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Gets the specified folder tree.",
            Description = "Gets the specified folder tree.<br>" +
                "Criteria:<br>{<br>" +
                "&nbsp;&nbsp;&nbsp;&nbsp;rootId: string<br>" +
                "<br>}<br>" +
                "Result: FolderNodeDto",
            OperationId = "FolderTree.View",
            Tags = new[] { "Tree" })
        ]
        public override async Task<ActionResult> HandleAsync(
            string id,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var tree = await FolderTree.Get(Csla.Factory, id);
                return Ok(tree.ToDto<FolderNodeDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
