using Csla6ModelTemplates.Contracts.Tree.View;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Models.Tree.Choice;
using Csla6ModelTemplates.Models.Tree.View;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi.Controllers
{
    /// <summary>
    /// Contains the API endpoints for trees.
    /// </summary>
    [Route("api/tree")]
    [ApiController]
    [Produces("application/json")]
    public class TreeController : ApiController
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public TreeController(
            ILogger<TreeController> logger,
            ICslaService csla
            ) : base(logger, csla)
        { }

        #endregion

        #region Choice

        /// <summary>
        /// Gets the ID-name choice of the trees.
        /// </summary>
        /// <returns>The ID-name choice of the trees.</returns>
        [HttpGet("choice")]
        [ProducesResponseType(typeof(List<IdNameOptionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRootFolderChoice()
        {
            try
            {
                var choice = await RootFolderChoice.Get(Factory);
                return Ok(choice.ToDto<IdNameOptionDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region View

        /// <summary>
        /// Gets the specified folder tree.
        /// </summary>
        /// <param name="id">The identifier of the root folder.</param>
        /// <returns>The requested folder tree.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<FolderNodeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFolderTree(
            string id
            )
        {
            try
            {
                var tree = await FolderTree.Get(Factory, id);
                return Ok(tree.ToDto<FolderNodeDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion
    }
}
