using Csla6ModelTemplates.Contracts.Selection.WithCode;
using Csla6ModelTemplates.Contracts.Selection.WithId;
using Csla6ModelTemplates.Contracts.Selection.WithKey;
using Csla6ModelTemplates.CslaExtensions.Utilities;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Models.Selection.WithCode;
using Csla6ModelTemplates.Models.Selection.WithId;
using Csla6ModelTemplates.Models.Selection.WithKey;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi.Controllers
{
    /// <summary>
    /// Contains the API endpoints for selections.
    /// </summary>
    [ApiController]
    [Route("api/selection")]
    [Produces("application/json")]
    public class SelectionController : ApiController
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="csla">The CSLA helper service.</param>
        public SelectionController(
            ILogger<SelectionController> logger,
            ICslaService csla
            ) : base(logger, csla)
        { }

        #endregion

        #region Choice with key

        /// <summary>
        /// Gets the key-name choice of the teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team choice.</param>
        /// <returns>The key-name choice of the teams.</returns>
        [HttpGet("with-key")]
        [ProducesResponseType(typeof(List<KeyNameOptionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeamChoiceWithKey(
            [FromQuery] TeamKeyChoiceCriteria criteria
            )
        {
            try
            {
                var choice = await TeamKeyChoice.Get(Factory, criteria);
                return Ok(choice.ToDto<KeyNameOptionDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Choice with ID

        /// <summary>
        /// Gets the ID-name choice of the teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team choice.</param>
        /// <returns>The ID-name choice of the teams.</returns>
        [HttpGet("with-id")]
        [ProducesResponseType(typeof(List<IdNameOptionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeamChoiceWithId(
            [FromQuery] TeamIdChoiceCriteria criteria
            )
        {
            try
            {
                var choice = await TeamIdChoice.Get(Factory, criteria);
                return Ok(choice.ToDto<IdNameOptionDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion

        #region Choice with code

        /// <summary>
        /// Gets the code-name choice of the teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team choice.</param>
        /// <returns>The code-name choice of the tenants.</returns>
        [HttpGet("with-code")]
        [ProducesResponseType(typeof(List<CodeNameOptionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTeamChoiceWithCode(
            [FromQuery] TeamCodeChoiceCriteria criteria
            )
        {
            try
            {
                var choice = await TeamCodeChoice.Get(Factory, criteria);
                return Ok(choice.ToDto<CodeNameOptionDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion
    }
}
