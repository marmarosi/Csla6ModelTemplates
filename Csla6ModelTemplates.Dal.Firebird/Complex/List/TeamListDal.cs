using Csla6ModelTemplates.Contracts.Complex.List;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.Firebird.Complex.List
{
    /// <summary>
    /// Implements the data access functions of the read-only team collection.
    /// </summary>
    [DalImplementation]
    public class TeamListDal : DalBase<FirebirdContext>, ITeamListDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public TeamListDal(
            FirebirdContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Fetch

        /// <summary>
        /// Gets the specified teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <returns>The requested team items.</returns>
        public List<TeamListItemDao> Fetch(
            TeamListCriteria criteria
            )
        {
            var list = DbContext.Teams
                .Include(e => e.Players)
                .Where(e =>
                    criteria.TeamName == null || e.TeamName.Contains(criteria.TeamName)
                )
                .Select(e => new TeamListItemDao
                {
                    TeamKey = e.TeamKey,
                    TeamCode = e.TeamCode,
                    TeamName = e.TeamName,
                    Players = e.Players.Select(i => new PlayerInfoDao
                    {
                        PlayerKey = i.PlayerKey,
                        PlayerCode = i.PlayerCode,
                        PlayerName = i.PlayerName
                    })
                    .OrderBy(io => io.PlayerName)
                    .ToList()
                })
                .OrderBy(o => o.TeamName)
                .AsNoTracking()
                .ToList();

            return list;
        }

        #endregion GetList
    }
}
