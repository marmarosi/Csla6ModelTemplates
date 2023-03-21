using Csla;
using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.Contracts.Simple.List;
using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.CslaExtensions;
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
        /// <param name="csla">The CSLA helper service.</param>
        public SimpleController(
            ILogger<SimpleController> logger,
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
        [ProducesResponseType(typeof(List<SimpleTeamListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SimpleTeamListItemDto>>> GetTeamList(
            [FromQuery] SimpleTeamListCriteria criteria
            )
        {
            try
            {
                var list = await SimpleTeamList.Get(Factory, criteria);
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
        /// <returns>The requested team view.</returns>
        [HttpGet("{id}/view")]
        [ProducesResponseType(typeof(SimpleTeamViewDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamViewDto>> GetTeamView(
            string id
            )
        {
            try
            {
                var team = await SimpleTeamView.Get(Factory, id);
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
        /// <returns>The new team.</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamDto>> GetNewTeam()
        {
            try
            {
                var team = await SimpleTeam.New(Factory);
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
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<SimpleTeamDto>> CreateTeam(
            [FromBody] SimpleTeamDto dto
            )
        {
            try
            {
                return await Call<SimpleTeamDto>.RetryOnDeadlock(async () =>
                {
                    var team = await SimpleTeam.Build(Factory, ChildFactory, dto);
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

        #region Read

        /// <summary>
        /// Gets the specified team to edit.
        /// </summary>
        /// <param name="id">The identifier of the team.</param>
        /// <returns>The requested team.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamDto>> GetTeam(
            string id
            )
        {
            try
            {
                var team = await SimpleTeam.Get(Factory, id);
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
        [ProducesResponseType(typeof(SimpleTeamDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleTeamDto>> UpdateTeam(
            [FromBody] SimpleTeamDto dto
            )
        {
            try
            {
                return await Call<SimpleTeamDto>.RetryOnDeadlock(async () =>
                {
                    var team = await SimpleTeam.Build(Factory, ChildFactory, dto);
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
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteTeam(
            string id
            )
        {
            try
            {
                return await Run.RetryOnDeadlock(async () =>
                {
                    await SimpleTeam.Delete(Factory, id);
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
        /// <returns>The requested team set.</returns>
        [HttpGet("set")]
        [ProducesResponseType(typeof(List<SimpleTeamSetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SimpleTeamSetItemDto>>> GetTeamSet(
            [FromQuery] SimpleTeamSetCriteria criteria
            )
        {
            try
            {
                var teams = await SimpleTeamSet.Get(Factory, criteria);
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
        /// <param name="factory">The data portal factory of the collection.</param>
        /// <param name="childFactory">The data portal factory of the items.</param>
        /// <returns>The updated team set.</returns>
        [HttpPut("set")]
        [ProducesResponseType(typeof(List<SimpleTeamSetItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SimpleTeamSetItemDto>>> UpdateTeamSet(
            [FromQuery] SimpleTeamSetCriteria criteria,
            [FromBody] List<SimpleTeamSetItemDto> dto,
            [FromServices] IDataPortalFactory factory,
            [FromServices] IChildDataPortalFactory childFactory
            )
        {
            try
            {
                return await Call<List<SimpleTeamSetItemDto>>.RetryOnDeadlock(async () =>
                {
                    var teams = await SimpleTeamSet.Build(Factory, ChildFactory, criteria, dto);
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
        /// Renames the specified team.
        /// </summary>
        /// <param name="dto">The data transer object of the rename team command.</param>
        /// <returns>True when the team was renamed; otherwise false.</returns>
        [HttpPatch]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> RenameTeamCommand(
            [FromBody] RenameTeamDto dto
            )
        {
            try
            {
                return await Run.RetryOnDeadlock(async () =>
                {
                    var command = await RenameTeam.Execute(Factory, dto);
                    return Ok(command.Result);
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion
    }
}
