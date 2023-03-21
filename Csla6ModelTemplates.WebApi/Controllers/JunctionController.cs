using Csla;
using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.Contracts.Junction.View;
using Csla6ModelTemplates.CslaExtensions;
using Csla6ModelTemplates.Dal;
using Csla6ModelTemplates.Models.Junction.Edit;
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
        /// <param name="csla">The CSLA helper service.</param>
        public JunctionController(
            ILogger<JunctionController> logger,
            ICslaService csla
            ) : base(logger, csla)
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
            string id
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

        #region New

        /// <summary>
        /// Gets a new group to edit.
        /// </summary>
        /// <returns>A new group.</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GroupDto>> GetNewGroup()
        {
            try
            {
                Group group = await Group.New(Factory);
                return Ok(group.ToDto());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Read

        /// <summary>
        /// Gets the specified group to edit.
        /// </summary>
        /// <param name="id">The identifier of the group.</param>
        /// <returns>The requested group.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GroupDto>> GetGroup(
            string id
            )
        {
            try
            {
                Group group = await Group.Get(Factory, id);
                return Ok(group.ToDto());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new group.
        /// </summary>
        /// <param name="dto">The data transer object of the group.</param>
        /// <returns>The created group.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<GroupDto>> CreateGroup(
            [FromBody] GroupDto dto
            )
        {
            try
            {
                return await Call<GroupDto>.RetryOnDeadlock(async () =>
                {
                    Group group = await Group.Build(Factory, ChildFactory, dto);
                    if (group.IsValid)
                    {
                        group = await group.SaveAsync();
                    }
                    return Created(Uri, group.ToDto());
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
        /// Updates the specified group.
        /// </summary>
        /// <param name="dto">The data transer object of the group.</param>
        /// <returns>The updated group.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GroupDto>> UpdateGroup(
            [FromBody] GroupDto dto
            )
        {
            try
            {
                return await Call<GroupDto>.RetryOnDeadlock(async () =>
                {
                    Group group = await Group.Build(Factory, ChildFactory, dto);
                    if (group.IsSavable)
                    {
                        group = await group.SaveAsync();
                    }
                    return Ok(group.ToDto());
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
        /// Deletes the specified group.
        /// </summary>
        /// <param name="id">The identifier of the group.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteGroup(
            string id
            )
        {
            try
            {
                return await Run.RetryOnDeadlock(async () =>
                {
                    await Task.Run(() => Group.Delete(Factory, id));
                    return NoContent();
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
