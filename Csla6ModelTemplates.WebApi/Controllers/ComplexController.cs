using Csla;
using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.Contracts.Complex.View;
using Csla6ModelTemplates.Models.Complex.Command;
using Csla6ModelTemplates.Models.Complex.List;
using Csla6ModelTemplates.Models.Complex.View;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi.Controllers
{
    /// <summary>
    /// Contains the API endpoints for complex models.
    /// </summary>
    [ApiController]
    [Route("api/complex")]
    [Produces("application/json")]
    public class ComplexController : ApiController
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        public ComplexController(
            ILogger<ComplexController> logger
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
        [ProducesResponseType(typeof(List<TeamListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TeamListItemDto>>> GetTeamList(
            [FromQuery] TeamListCriteria criteria,
            [FromServices] IDataPortal<TeamList> portal
            )
        {
            try
            {
                TeamList list = await portal.FetchAsync(criteria);
                return Ok(list.ToDto<TeamListItemDto>());
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
        [HttpGet("{id}/view")]
        [ProducesResponseType(typeof(TeamViewDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<TeamViewDto>> GetTeamView(
            string id,
            [FromServices] IDataPortal<TeamView> portal
            )
        {
            try
            {
                var criteria = new TeamViewParams(id);
                var team = await portal.FetchAsync(criteria.Decode());
                return Ok(team.ToDto<TeamViewDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Counts the teams grouped by the number of their items.
        /// </summary>
        /// <param name="criteria">The criteria of the count teams by item count command.</param>
        /// <param name="portal">The data portal of the model.</param>
        /// <returns>The list of the team counts.</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(List<CountTeamsResultDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CountTeamsResultDto>>> CountTeamsCommand(
            [FromBody] CountTeamsCriteria criteria,
            [FromServices] IDataPortal<CountTeams> portal
            )
        {
            try
            {
                CountTeams command = await portal.ExecuteAsync(criteria);
                return Ok(command.Results.ToDto<CountTeamsResultDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion
    }
}
