using Csla;
using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.Models.Simple.List;
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

        #region List

        /// <summary>
        /// Gets a list of teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <param name="portal">The data portal of the collection.</param>
        /// <returns>The requested team list.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<SimpleTeamListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SimpleTeamListItemDto>>> GetTeamList(
            [FromQuery] SimpleTeamListCriteria criteria,
            [FromServices] IDataPortal<SimpleTeamList> portal
            )
        {
            try
            {
                SimpleTeamList list = await portal.FetchAsync(criteria);
                return Ok(list.ToDto<SimpleTeamListItemDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region View

        /// <summary>
        /// Gets the specified team details to display.
        /// </summary>
        /// <param name="id">The identifier of the team.</param>
        /// <param name="portal">The data portal of the model.</param>
        /// <returns>The requested team view.</returns>
        [HttpGet("view/{id}")]
        [ProducesResponseType(typeof(SimpleTeamViewDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamViewDto>> GetTeamView(
            string id,
            [FromServices] IDataPortal<SimpleTeamView> portal
            )
        {
            try
            {
                var criteria = new SimpleTeamViewParams(id);
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
