using Csla;
using Csla6ModelTemplates.Contracts.Junction.View;
using Csla6ModelTemplates.Models.Junction.View;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi.Controllers
{
    /// <summary>
    /// Contains the API endpoints for junction models.
    /// </summary>
    [ApiController]
    [Route("api/junction")]
    [Produces("application/json")]
    public class JunctionController : ApiController
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        public JunctionController(
            ILogger<JunctionController> logger,
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory
            ) : base(logger, factory, childFactory)
        { }

        #endregion

        #region View

        /// <summary>
        /// Gets the specified group details to display.
        /// </summary>
        /// <param name="id">The identifier of the group.</param>
        /// <returns>The requested group view.</returns>
        [HttpGet("{id}/view")]
        [ProducesResponseType(typeof(GroupViewDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GroupViewDto>> GetGroupView(
            [FromQuery] string id
            )
        {
            try
            {
                GroupView group = await GroupView.Get(Factory, id);
                return Ok(group.ToDto<GroupViewDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion
    }
}
