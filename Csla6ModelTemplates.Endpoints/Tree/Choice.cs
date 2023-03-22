using Ardalis.ApiEndpoints;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Csla6ModelTemplates.Models.Tree.Choice;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Csla6ModelTemplates.Endpoints.Tree
{
    /// <summary>
    /// Gets the ID-name choice of the teams.
    /// </summary>
    [Route(Routes.Tree)]
    public class Choice : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IList<IdNameOptionDto>>
    {
        internal ILogger Logger { get; private set; }
        internal ICslaService Csla { get; private set; }

        /// <summary>
        /// Creates a new instance of the endpoint.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public Choice(
            ILogger<Choice> logger,
            ICslaService csla
            )
        {
            Logger = logger;
            Csla = csla;
        }

        /// <summary>
        /// Gets the ID-name choice of the trees.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The ID-name choice of the trees.</returns>
        [HttpGet("choice")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Gets the ID-name choice of the trees.",
            Description = "Gets the ID-name choice of the teams.<br>" +
                "Result: IdNameOptionDto[]",
            OperationId = "FolderTree.Choice",
            Tags = new[] { "Tree" })
        ]
        public override async Task<ActionResult<IList<IdNameOptionDto>>> HandleAsync(
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                var choice = await RootFolderChoice.Get(Csla.Factory);
                return Ok(choice.ToDto<IdNameOptionDto>());
            }
            catch (Exception ex)
            {
                return Helper.HandleError(this, Logger, Csla.DeadLock, ex);
            }
        }
    }
}
