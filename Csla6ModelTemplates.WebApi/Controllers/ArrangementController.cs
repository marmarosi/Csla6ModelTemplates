using Csla;
using Csla6ModelTemplates.Contracts.Arrangement.Sorting;
using Csla6ModelTemplates.Models.Arrangement.Sorting;
using Microsoft.AspNetCore.Mvc;

namespace Csla6ModelTemplates.WebApi.Controllers
{
    /// <summary>
    /// Contains the API endpoints for sorting and pagination.
    /// </summary>
    [ApiController]
    [Route("api/arrangement")]
    [Produces("application/json")]
    public class ArrangementController : ApiController
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">The application logging service.</param>
        /// <param name="factory">The data portal factory.</param>
        /// <param name="childFactory">The child data portal factory.</param>
        public ArrangementController(
            ILogger<ArrangementController> logger,
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory
            ) : base(logger, factory, childFactory)
        { }

        #endregion

        #region Sorting

        /// <summary>
        /// Gets the specified teams sorted.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <returns>The requested team list.</returns>
        [HttpGet("sorted")]
        [ProducesResponseType(typeof(List<SortedTeamListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SortedTeamListItemDto>>> GetSortedTeamList(
            [FromQuery] SortedTeamListCriteria criteria
            )
        {
            try
            {
                SortedTeamList list = await SortedTeamList.Get(Factory, criteria);
                return Ok(list.ToDto<SortedTeamListItemDto>());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        #endregion
    }
}
