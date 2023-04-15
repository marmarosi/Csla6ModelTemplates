using Csla6ModelTemplates.Contracts.Complex.Command;
using Csla6ModelTemplates.Dal.Exceptions;
using Csla6ModelTemplates.Resources;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.Firebird.Complex.Command
{
    /// <summary>
    /// Implements the data access functions of the count teams by player count command.
    /// </summary>
    [DalImplementation]
    public class CountTeamsDal : DalBase<FirebirdContext>, ICountTeamsDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CountTeamsDal(
            FirebirdContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Execute

        /// <summary>
        /// Counts the teams grouped by the number of their players.
        /// </summary>
        /// <param name="criteria">The criteria of the command.</param>
        public List<CountTeamsResultDao> Execute(
            CountTeamsCriteria criteria
            )
        {
            string teamName = criteria.TeamName ?? "";

            var counts = DbContext.Teams
                .Include(e => e.Players)
                .Where(e => teamName == "" || e.TeamName.Contains(teamName))
                .Select(e => new { e.TeamKey, Count = e.Players.Count })
                .AsNoTracking()
                .ToList();

            var list = counts
                .GroupBy(
                    e => e.Count,
                    (key, grp) => new CountTeamsResultDao
                    {
                        ItemCount = key,
                        CountOfTeams = grp.Count()
                    })
                .OrderByDescending(o => o.ItemCount)
                .ToList();

            if (list.Count == 0)
                throw new CommandFailedException(DalText.CountTeams_CountFailed);

            return list;
        }

        #endregion
    }
}
