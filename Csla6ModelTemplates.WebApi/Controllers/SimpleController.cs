using Csla;
using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.Models.Simple.View;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi.Controllers
{
    /// <summary>
    /// Contains the API endpoints for simple models.
    /// </summary>
    [Route("api/simple")]
    [ApiController]
    public class SimpleController : ApiController
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        public SimpleController(
            ILogger<SimpleController> logger
            ) : base(logger)
        { }

        #endregion

        #region View

        /// <summary>
        /// Gets the specified team details to display.
        /// </summary>
        /// <param name="criteria">The criteria of the team view.</param>
        /// <param name="portal">The data portal of the model.</param>
        /// <returns>The requested team view.</returns>
        [HttpGet("view")]
        [ProducesResponseType(typeof(SimpleTeamViewDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamViewDto>> GetTeamView(
            [FromQuery] SimpleTeamViewParams criteria,
            [FromServices] IDataPortal<SimpleTeamView> portal
            )
        {
            try
            {
                // NbeoGDyVEOp
                var team = await portal.FetchAsync(criteria.Decode());
                return Ok(team.ToDto<SimpleTeamViewDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion
    }
}
