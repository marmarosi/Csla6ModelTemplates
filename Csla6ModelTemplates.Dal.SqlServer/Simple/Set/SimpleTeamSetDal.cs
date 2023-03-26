using Csla6ModelTemplates.Contracts.Simple.Set;

namespace Csla6ModelTemplates.Dal.SqlServer.Simple.Set
{
    /// <summary>
    /// Implements the data access functions of the editable team collection.
    /// </summary>
    [DalImplementation]
    public class SimpleTeamSetDal : DalBase<SqlServerContext>, ISimpleTeamSetDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SimpleTeamSetDal(
            SqlServerContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Fetch

        /// <summary>
        /// Gets the specified team set.
        /// </summary>
        /// <param name="criteria">The criteria of the team set.</param>
        /// <returns>The requested team set.</returns>
        public List<SimpleTeamSetItemDao> Fetch(
            SimpleTeamSetCriteria criteria
            )
        {
            // Get the specified team set.
            var list = DbContext.Teams
                .Where(e =>
                    criteria.TeamName == null || e.TeamName.Contains(criteria.TeamName)
                )
                .Select(e => new SimpleTeamSetItemDao
                {
                    TeamKey = e.TeamKey,
                    TeamCode = e.TeamCode,
                    TeamName = e.TeamName,
                    Timestamp = e.Timestamp
                })
                .OrderBy(o => o.TeamName)
                .ToList();

            return list;
        }

        #endregion Fetch
    }
}
