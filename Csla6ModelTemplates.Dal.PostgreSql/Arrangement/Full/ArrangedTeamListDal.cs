using Csla6ModelTemplates.Contracts.Arrangement.Full;
using Csla6ModelTemplates.Dal.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.PostgreSql.Arrangement.Full
{
    /// <summary>
    /// Implements the data access functions of the read-only paginated sorted team collection.
    /// </summary>
    [DalImplementation]
    public class ArrangedTeamListDal : DalBase<PostgreSqlContext>, IArrangedTeamListDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ArrangedTeamListDal(
            PostgreSqlContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Fetch

        /// <summary>
        /// Gets the specified page of sorted teams.
        /// </summary>
        /// <param name="criteria">The criteria of the team list.</param>
        /// <returns>The requested page of the sorted team list.</returns>
        public IPaginatedList<ArrangedTeamListItemDao> Fetch(
            ArrangedTeamListCriteria criteria
            )
        {
            // Filter the teams.
            var query = DbContext.Teams
                .Where(e =>
                    criteria.TeamName == null || e.TeamName.Contains(criteria.TeamName)
                );

            // Sort the items.
            var sorted = query
                .Select(e => new ArrangedTeamListItemDao
                {
                    TeamKey = e.TeamKey,
                    TeamCode = e.TeamCode,
                    TeamName = e.TeamName
                });

            switch (criteria.SortBy)
            {
                case ArrangedTeamListSortBy.TeamCode:
                    sorted = criteria.SortDirection == SortDirection.Ascending
                        ? sorted.OrderBy(e => e.TeamCode)
                        : sorted.OrderByDescending(e => e.TeamCode);
                    break;
                // case ArrangedTeamListSortBy.TeamName:
                default:
                    sorted = criteria.SortDirection == SortDirection.Ascending
                        ? sorted.OrderBy(e => e.TeamName)
                        : sorted.OrderByDescending(e => e.TeamName);
                    break;
            }

            // Get the requested page.
            var list = sorted
                .Skip(criteria.PageIndex * criteria.PageSize)
                .Take(criteria.PageSize)
                .AsNoTracking()
                .ToList();

            // Count the matching teams.
            int totalCount = query.Count();

            // Return the result.
            return new PaginatedList<ArrangedTeamListItemDao>
            {
                Data = list,
                TotalCount = totalCount
            };
        }

        #endregion GetList
    }
}
