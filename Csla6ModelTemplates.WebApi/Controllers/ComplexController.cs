using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.Contracts.Complex.List;
using Csla6ModelTemplates.Contracts.Complex.Set;
using Csla6ModelTemplates.Contracts.Complex.View;
using Csla6ModelTemplates.CslaExtensions.Utilities;
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
        /// <param name="csla">The CSLA helper service.</param>
        public ComplexController(
            ILogger<ComplexController> logger,
            ICslaService csla
            ) : base(logger, csla)
        { }

        #endregion

        #region List

        /// <summary>
        /// Gets a list of teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <returns>The requested team list.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TeamListItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeamList(
            [FromQuery] TeamListCriteria criteria
            )
        {
            try
            {
                var list = await TeamList.Get(Factory, criteria);
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
        /// <returns>The requested team view.</returns>
        [HttpGet("{id}/view")]
        [ProducesResponseType(typeof(TeamViewDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeamView(
            string id
            )
        {
            try
            {
                var team = await TeamView.Get(Factory, id);
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
        /// <returns>The new team.</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNewTeam()
        {
            try
            {
                var team = await Team.New(Factory);
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
        /// <returns>The created team.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTeam(
            [FromBody] TeamDto dto
            )
        {
            try
            {
                return Created(Uri, await RetryOnDeadlock(async () =>
                {
                    var team = await Team.Build(Factory, ChildFactory, dto);
                    if (team.IsValid)
                    {
                        team = await team.SaveAsync();
                    }
                    return team.ToDto();
                }));
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
        /// <returns>The requested team.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeam(
            string id
            )
        {
            try
            {
                var team = await Team.Get(Factory, id);
                return Ok(team.ToDto());
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
        /// <returns>The updated team.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTeam(
            [FromBody] TeamDto dto
            )
        {
            try
            {
                return Ok(await RetryOnDeadlock(async () =>
                {
                    var team = await Team.Build(Factory, ChildFactory, dto);
                    if (team.IsSavable)
                    {
                        team = await team.SaveAsync();
                    }
                    return team.ToDto();
                }));
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
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTeam(
            string id
            )
        {
            try
            {
                await RetryOnDeadlock(async () =>
                {
                    await Team.Delete(Factory, id);
                });
                return NoContent();
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
        /// <returns>The requested team set.</returns>
        [HttpGet("set")]
        [ProducesResponseType(typeof(IList<TeamSetItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeamSet(
            [FromQuery] TeamSetCriteria criteria
            )
        {
            try
            {
                var teams = await TeamSet.Get(Factory, criteria);
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
        /// <returns>The updated team set.</returns>
        [HttpPut("set")]
        [ProducesResponseType(typeof(IList<TeamSetItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTeamSet(
            [FromQuery] TeamSetCriteria criteria,
            [FromBody] List<TeamSetItemDto> dto
            )
        {
            try
            {
                return Ok(await RetryOnDeadlock(async () =>
                {
                    var teams = await TeamSet.Build(Factory, ChildFactory, criteria, dto);
                    if (teams.IsSavable)
                    {
                        teams = await teams.SaveAsync();
                    }
                    return teams.ToDto();
                }));
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
        /// <returns>The list of the team counts.</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(List<CountTeamsResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CountTeamsCommand(
            [FromBody] CountTeamsCriteria criteria
            )
        {
            try
            {
                var command = await CountTeams.Execute(Factory, criteria);
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
