using Csla6ModelTemplates.Contracts.Simple.View;
using Csla6ModelTemplates.Dal.Exceptions;
using Csla6ModelTemplates.Resources;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.SqlServer.Simple
{
    /// <summary>
    /// Implements the data access functions of the read-only team model.
    /// </summary>
    public partial class SimpleDal : ISimpleTeamViewDal
    {
        /// <summary>
        /// Gets the specified team view.
        /// </summary>
        /// <param name="criteria">The criteria of the team.</param>
        /// <returns>The requested team view.</returns>
        public SimpleTeamViewDao GetView(
            SimpleTeamViewCriteria criteria
            )
        {
            // Get the specified team.
            SimpleTeamViewDao team = Context.Teams
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
                .FirstOrDefault();

            if (team == null)
                throw new DataNotFoundException(DalText.SimpleTeam_NotFound);

            return team;
        }
    }
}
