using Csla;
using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.Models.Simple.Command;
using Csla6ModelTemplates.Models.Simple.Edit;
using Csla6ModelTemplates.Models.Simple.List;
using Csla6ModelTemplates.Models.Simple.Set;
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
        [HttpGet("{id}/view")]
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

        #region New

        /// <summary>
        /// Gets a new team to edit.
        /// </summary>
        /// <param name="portal">The data portal of the model.</param>
        /// <returns>The new team.</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamDto>> GetNewTeam(
            [FromServices] IDataPortal<SimpleTeam> portal
            )
        {
            try
            {
                SimpleTeam team = await portal.CreateAsync();
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
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamDto>> GetTeam(
            string id,
            [FromServices] IDataPortal<SimpleTeam> portal
            )
        {
            try
            {
                var criteria = new SimpleTeamParams(id);
                SimpleTeam team = await portal.FetchAsync(criteria.Decode());
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
        /// <returns>The created team.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<SimpleTeamDto>> CreateTeam(
            [FromBody] SimpleTeamDto dto,
            [FromServices] IDataPortal<SimpleTeam> portal
            )
        {
            try
            {
                return await Call<SimpleTeamDto>.RetryOnDeadlock(async () =>
                {
                    SimpleTeam team = await SimpleTeam.FromDto(dto, portal);
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
        /// <returns>The updated team.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamDto>> UpdateTeam(
            [FromBody] SimpleTeamDto dto,
            [FromServices] IDataPortal<SimpleTeam> portal
            )
        {
            try
            {
                return await Call<SimpleTeamDto>.RetryOnDeadlock(async () =>
                {
                    SimpleTeam team = await SimpleTeam.FromDto(dto, portal);
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
            [FromServices] IDataPortal<SimpleTeam> portal
            )
        {
            try
            {
                var criteria = new SimpleTeamParams(id);
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
        /// <param name="portal">The data portal of the model.</param>
        /// <returns>The requested team set.</returns>
        [HttpGet("set")]
        [ProducesResponseType(typeof(List<SimpleTeamSetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SimpleTeamSetItemDto>>> GetTeamSet(
            [FromQuery] SimpleTeamSetCriteria criteria,
            [FromServices] IDataPortal<SimpleTeamSet> portal
            )
        {
            try
            {
                SimpleTeamSet set = await portal.FetchAsync(criteria);
                return Ok(set.ToDto());
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
        [ProducesResponseType(typeof(List<SimpleTeamSetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SimpleTeamSetItemDto>>> UpdateTeamSet(
            [FromQuery] SimpleTeamSetCriteria criteria,
            [FromBody] List<SimpleTeamSetItemDto> dto,
            [FromServices] IDataPortal<SimpleTeamSet> portal,
            [FromServices] IChildDataPortal<SimpleTeamSetItem> itemPortal
            )
        {
            try
            {
                return await Call<List<SimpleTeamSetItemDto>>.RetryOnDeadlock(async () =>
                {
                    SimpleTeamSet team = await SimpleTeamSet.FromDto(criteria, dto, portal, itemPortal);
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

        #region Rename

        /// <summary>
        /// Renames the specified team.
        /// </summary>
        /// <param name="dto">The data transer object of the rename team command.</param>
        /// <param name="portal">The data portal of the model.</param>
        /// <returns>True when the team was renamed; otherwise false.</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> RenameTeamCommand(
            [FromBody] RenameTeamDto dto,
            [FromServices] IDataPortal<RenameTeam> portal
            )
        {
            try
            {
                RenameTeam command = await portal.ExecuteAsync(dto);
                return Ok(command.Result);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion
    }
}
