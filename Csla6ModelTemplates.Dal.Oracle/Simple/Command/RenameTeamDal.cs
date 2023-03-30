using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.Dal.Exceptions;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.Dal.Oracle.Simple.Command
{
    /// <summary>
    /// Implements the data access functions of the rename team command.
    /// </summary>
    [DalImplementation]
    public class RenameTeamDal : DalBase<OracleContext>, IRenameTeamDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public RenameTeamDal(
            OracleContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Execute

        /// <summary>
        /// Sets the new name of the specified team.
        /// </summary>
        /// <param name="dao">The data of the command.</param>
        public void Execute(
            RenameTeamDao dao
            )
        {
            // Get the specified team.
            var team = DbContext.Teams
                .Where(e => e.TeamKey == dao.TeamKey)
                .FirstOrDefault()
                ?? throw new DataNotFoundException(DalText.RenameTeam_NotFound);

            // Update the team.
            team.TeamName = dao.TeamName;

            int count = DbContext.SaveChanges();
            if (count == 0)
                throw new CommandFailedException(DalText.RenameTeam_RenameFailed);
        }

        #endregion
    }
}
