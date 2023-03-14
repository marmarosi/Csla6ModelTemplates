using Csla;
using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.Contracts.Complex.View;
using Csla6ModelTemplates.Models.Complex.Command;
using Csla6ModelTemplates.Models.Complex.Edit;
using Csla6ModelTemplates.Models.Complex.List;
using Csla6ModelTemplates.Models.Complex.Set;
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

        #region New

        /// <summary>
        /// Gets a new team to edit.
        /// </summary>
        /// <param name="portal">The data portal of the model.</param>
        /// <returns>The new team.</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<TeamDto>> GetNewTeam(
            [FromServices] IDataPortal<Team> portal
            )
        {
            try
            {
                Team team = await portal.CreateAsync();
                return Ok(team.ToDto());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Read

        /// <summary>
        /// Gets the specified team to edit.
        /// </summary>
        /// <param name="id">The identifier of the team.</param>
        /// <param name="portal">The data portal of the model.</param>
        /// <returns>The requested team.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<TeamDto>> GetTeam(
            string id,
            [FromServices] IDataPortal<Team> portal
            )
        {
            try
            {
                var criteria = new TeamParams(id);
                Team team = await portal.FetchAsync(criteria.Decode());
                return Ok(team.ToDto());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="dto">The data transer object of the team.</param>
        /// <param name="portal">The data portal of the model.</param>
        /// <param name="itemPortal">The data portal of the item model.</param>
        /// <returns>The created team.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<TeamDto>> CreateTeam(
            [FromBody] TeamDto dto,
            [FromServices] IDataPortal<Team> portal,
            [FromServices] IChildDataPortal<Player> itemPortal
            )
        {
            try
            {
                return await Call<TeamDto>.RetryOnDeadlock(async () =>
                {
                    Team team = await Team.FromDto(dto, portal, itemPortal);
                    if (team.IsValid)
                    {
                        team = await team.SaveAsync();
                    }
                    return Created(Uri, team.ToDto());
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the specified team.
        /// </summary>
        /// <param name="dto">The data transer object of the team.</param>
        /// <param name="portal">The data portal of the model.</param>
        /// <param name="itemPortal">The data portal of the item model.</param>
        /// <returns>The updated team.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<TeamDto>> UpdateTeam(
            [FromBody] TeamDto dto,
            [FromServices] IDataPortal<Team> portal,
            [FromServices] IChildDataPortal<Player> itemPortal
            )
        {
            try
            {
                return await Call<TeamDto>.RetryOnDeadlock(async () =>
                {
                    Team team = await Team.FromDto(dto, portal, itemPortal);
                    if (team.IsSavable)
                    {
                        team = await team.SaveAsync();
                    }
                    return Ok(team.ToDto());
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the specified team.
        /// </summary>
        /// <param name="id">The identifier of the team.</param>
        /// <param name="portal">The data portal of the model.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteTeam(
            string id,
            [FromServices] IDataPortal<Team> portal
            )
        {
            try
            {
                var criteria = new TeamParams(id);
                return await Run.RetryOnDeadlock(async () =>
                {
                    await portal.DeleteAsync(criteria.Decode());
                    return NoContent();
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Read-Set

        /// <summary>
        /// Gets the specified team set to edit.
        /// </summary>
        /// <param name="criteria">The criteria of the team set.</param>
        /// <param name="portal">The data portal of the collection.</param>
        /// <returns>The requested team set.</returns>
        [HttpGet("set")]
        [ProducesResponseType(typeof(List<TeamSetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TeamSetItemDto>>> GetTeamSet(
            [FromQuery] TeamSetCriteria criteria,
            [FromServices] IDataPortal<TeamSet> portal
            )
        {
            try
            {
                TeamSet teams = await portal.FetchAsync(criteria);
                return Ok(teams.ToDto());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Update-Set

        /// <summary>
        /// Updates the specified team set.
        /// </summary>
        /// <param name="criteria">The criteria of the team set.</param>
        /// <param name="dto">The data transer objects of the team set.</param>
        /// <param name="portal">The data portal of the collection.</param>
        /// <param name="itemPortal">The data portal of items.</param>
        /// <returns>The updated team set.</returns>
        [HttpPut("set")]
        [ProducesResponseType(typeof(List<TeamSetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TeamSetItemDto>>> UpdateTeamSet(
            [FromQuery] TeamSetCriteria criteria,
            [FromBody] List<TeamSetItemDto> dto,
            [FromServices] IDataPortal<TeamSet> portal,
            [FromServices] IChildDataPortal<TeamSetItem> itemPortal
            )
        {
            try
            {
                return await Call<List<TeamSetItemDto>>.RetryOnDeadlock(async () =>
                {
                    var set = portal.Create();
                    TeamSet teams = await TeamSet.FromDto(criteria, dto, portal, itemPortal);
                    if (teams.IsSavable)
                    {
                        teams = await teams.SaveAsync();
                    }
                    return Ok(teams.ToDto());
                });
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
