using Csla6ModelTemplates.Contracts.Complex.View;
using Csla6ModelTemplates.Dal.Exceptions;
using Csla6ModelTemplates.Resources;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.Db2.Complex.View
{
    /// <summary>
    /// Implements the data access functions of the read-only team object.
    /// </summary>
    [DalImplementation]
    public class TeamViewDal : DalBase<Db2Context>, ITeamViewDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TeamViewDal(
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
        public TeamViewDao Fetch(
            TeamViewCriteria criteria
            )
        {
            // Get the specified team.
            var team = DbContext.Teams
                .Include(e => e.Players)
                .Where(e =>
                    e.TeamKey == criteria.TeamKey
                 )
                .Select(e => new TeamViewDao
                {
                    TeamKey = e.TeamKey,
                    TeamCode = e.TeamCode,
                    TeamName = e.TeamName,
                    Players = e.Players
                        .Select(p => new PlayerViewDao
                        {
                            PlayerKey = p.PlayerKey,
                            PlayerCode = p.PlayerCode,
                            PlayerName = p.PlayerName
                        })
                    .OrderBy(p => p.PlayerName)
                    .ToList()
                })
                .AsNoTracking()
                .FirstOrDefault()
                ?? throw new DataNotFoundException(DalText.Team_NotFound);

            return team;
        }

        #endregion Fetch
    }
}
