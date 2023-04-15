using Csla6ModelTemplates.Contracts.Simple.List;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.Firebird.Simple.List
{
    /// <summary>
    /// Implements the data access functions of the read-only team collection.
    /// </summary>
    [DalImplementation]
    public class SimpleTeamListDal : DalBase<FirebirdContext>, ISimpleTeamListDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SimpleTeamListDal(
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
        public List<SimpleTeamListItemDao> Fetch(
            SimpleTeamListCriteria criteria
            )
        {
            var list = DbContext.Teams
                .Where(e =>
                    criteria.TeamName == null || e.TeamName.Contains(criteria.TeamName)
                )
                .Select(e => new SimpleTeamListItemDao
                {
                    TeamKey = e.TeamKey,
                    TeamCode = e.TeamCode,
                    TeamName = e.TeamName
                })
                .OrderBy(o => o.TeamName)
                .AsNoTracking()
                .ToList();

            return list;
        }

        #endregion Fetch
    }
}
