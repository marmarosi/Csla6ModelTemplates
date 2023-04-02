using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.Dal.Exceptions;
using Csla6ModelTemplates.Resources;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.Db2.Simple.View
{
    /// <summary>
    /// Implements the data access functions of the read-only team model.
    /// </summary>
    [DalImplementation]
    public partial class SimpleTeamViewDal : DalBase<Db2Context>, ISimpleTeamViewDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SimpleTeamViewDal(
            Db2Context dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Fetch

        /// <summary>
        /// Gets the specified team view.
        /// </summary>
        /// <param name="criteria">The criteria of the team.</param>
        /// <returns>The requested team view.</returns>
        public SimpleTeamViewDao Fetch(
            SimpleTeamViewCriteria criteria
            )
        {
            // Get the specified team.
            var team = DbContext.Teams
                .Where(e =>
                    e.TeamKey == criteria.TeamKey
                 )
                .Select(e => new SimpleTeamViewDao
                {
                    TeamKey = e.TeamKey,
                    TeamCode = e.TeamCode,
                    TeamName = e.TeamName
                })
                .AsNoTracking()
                .FirstOrDefault()
                ?? throw new DataNotFoundException(DalText.SimpleTeam_NotFound);

            return team;
        }

        #endregion Fetch
    }
}
