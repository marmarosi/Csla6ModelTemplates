using Csla6ModelTemplates.Contracts.Simple.Edit;
using Csla6ModelTemplates.Dal.Exceptions;
using Csla6ModelTemplates.Dal.Oracle.Entities;
using Csla6ModelTemplates.Resources;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.Oracle.Simple.Edit
{
    /// <summary>
    /// Implements the data access functions of the editable team object.
    /// </summary>
    [DalImplementation]
    public class SimpleTeamDal : DalBase<OracleContext>, ISimpleTeamDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SimpleTeamDal(
            OracleContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Fetch

        /// <summary>
        /// Gets the specified team.
        /// </summary>
        /// <param name="criteria">The criteria of the team.</param>
        /// <returns>The requested team.</returns>
        public SimpleTeamDao Fetch(
            SimpleTeamCriteria criteria
            )
        {
            // Get the specified team.
            var team = DbContext.Teams
                .Where(e =>
                    e.TeamKey == criteria.TeamKey
                 )
                .Select(e => new SimpleTeamDao
                {
                    TeamKey = e.TeamKey,
                    TeamCode = e.TeamCode,
                    TeamName = e.TeamName,
                    Timestamp = e.Timestamp
                })
                .AsNoTracking()
                .FirstOrDefault()
                ?? throw new DataNotFoundException(DalText.SimpleTeam_NotFound);

            return team;
        }

        #endregion Fetch

        #region Insert

        /// <summary>
        /// Creates a new team using the specified data.
        /// </summary>
        /// <param name="dao">The data of the team.</param>
        public void Insert(
            SimpleTeamDao dao
            )
        {
            // Check unique team code.
            var team = DbContext.Teams
                .Where(e =>
                    e.TeamCode == dao.TeamCode
                )
                .FirstOrDefault();
            if (team != null)
                throw new DataExistException(DalText.SimpleTeam_TeamCodeExists.With(dao.TeamCode));

            // Create the new team.
            team = new Team
            {
                TeamCode = dao.TeamCode,
                TeamName = dao.TeamName
            };
            DbContext.Teams.Add(team);

            int count = DbContext.SaveChanges();
            if (count == 0)
                throw new InsertFailedException(DalText.SimpleTeam_InsertFailed);

            // Return new data.
            dao.TeamKey = team.TeamKey;
            dao.Timestamp = team.Timestamp;
        }

        #endregion Insert

        #region Update

        /// <summary>
        /// Updates an existing team using the specified data.
        /// </summary>
        /// <param name="dao">The data of the team.</param>
        public void Update(
            SimpleTeamDao dao
            )
        {
            // Get the specified team.
            var team = DbContext.Teams
                .Where(e =>
                    e.TeamKey == dao.TeamKey
                )
                .FirstOrDefault()
                ?? throw new DataNotFoundException(DalText.SimpleTeam_NotFound);
            if (team.Timestamp != dao.Timestamp)
                throw new ConcurrencyException(DalText.SimpleTeam_Concurrency);

            // Check unique team code.
            if (team.TeamCode != dao.TeamCode)
            {
                int exist = DbContext.Teams
                    .Where(e => e.TeamCode == dao.TeamCode && e.TeamKey != team.TeamKey)
                    .Count();
                if (exist > 0)
                    throw new DataExistException(DalText.SimpleTeam_TeamCodeExists.With(dao.TeamCode));
            }

            // Update the team.
            team.TeamCode = dao.TeamCode;
            team.TeamName = dao.TeamName;

            int count = DbContext.SaveChanges();
            if (count == 0)
                throw new UpdateFailedException(DalText.SimpleTeam_UpdateFailed);

            // Return new data.
            dao.Timestamp = team.Timestamp;
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// Deletes the specified team.
        /// </summary>
        /// <param name="criteria">The criteria of the team.</param>
        public void Delete(
            SimpleTeamCriteria criteria
            )
        {
            int count = 0;

            // Get the specified team.
            var team = DbContext.Teams
                .Where(e =>
                    e.TeamKey == criteria.TeamKey
                 )
                .AsNoTracking()
                .FirstOrDefault()
                ?? throw new DataNotFoundException(DalText.SimpleTeam_NotFound);

            // Check references.
            //int dependents = 0;

            //dependents = DbContext.Others.Count(e => e.TeamKey == criteria.TeamKey);
            //if (dependents > 0)
            //    throw new DeleteFailedException(DalText.SimpleTeam_Delete_Others);

            // Delete references.
            var players = DbContext.Players
                .Where(e => e.TeamKey == criteria.TeamKey)
                .ToList();
            foreach (var player in players)
                DbContext.Players.Remove(player);

            count = DbContext.SaveChanges();
            if (count != players.Count)
                throw new DeleteFailedException(DalText.SimpleTeam_Delete_Players);

            // Delete the team.
            DbContext.Teams.Remove(team);

            count = DbContext.SaveChanges();
            if (count == 0)
                throw new DeleteFailedException(DalText.SimpleTeam_DeleteFailed);
        }

        #endregion Delete
    }
}
