using Csla6ModelTemplates.Contracts.Complex.Edit;
using Csla6ModelTemplates.Dal.Exceptions;
using Csla6ModelTemplates.Dal.PostgreSql.Entities;
using Csla6ModelTemplates.Resources;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.PostgreSql.Complex.Edit
{
    /// <summary>
    /// Implements the data access functions of the editable player object.
    /// </summary>
    [DalImplementation]
    public class PlayerDal : DalBase<PostgreSqlContext>, IPlayerDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public PlayerDal(
            PostgreSqlContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Insert

        /// <summary>
        /// Creates a new player using the specified data.
        /// </summary>
        /// <param name="dao">The data of the player.</param>
        public void Insert(
            PlayerDao dao
            )
        {
            // Check unique player code.
            var player = DbContext.Players
                .Where(e =>
                    e.TeamKey == dao.TeamKey &&
                    e.PlayerCode == dao.PlayerCode
                )
                .FirstOrDefault();
            if (player != null)
                throw new DataExistException(DalText.Player_PlayerCodeExists.With(dao.PlayerCode));

            // Create the new player.
            player = new Player
            {
                TeamKey = dao.TeamKey,
                PlayerCode = dao.PlayerCode,
                PlayerName = dao.PlayerName
            };
            DbContext.Players.Add(player);

            int count = DbContext.SaveChanges();
            if (count == 0)
                throw new InsertFailedException(DalText.Player_InsertFailed.With(player.PlayerCode));

            // Return new data.
            dao.PlayerKey = player.PlayerKey;
        }

        #endregion Insert

        #region Update

        /// <summary>
        /// Updates an existing player using the specified data.
        /// </summary>
        /// <param name="dao">The data of the player.</param>
        public void Update(
            PlayerDao dao
            )
        {
            // Get the specified player.
            var player = DbContext.Players
                .Where(e =>
                    e.PlayerKey == dao.PlayerKey
                )
                .FirstOrDefault()
                ?? throw new DataNotFoundException(DalText.Player_NotFound);

            // Check unique player code.
            if (player.PlayerCode != dao.PlayerCode)
            {
                int exist = DbContext.Players
                    .Where(e =>
                        e.TeamKey == dao.TeamKey &&
                        e.PlayerCode == dao.PlayerCode &&
                        e.PlayerKey != player.PlayerKey
                    )
                    .Count();
                if (exist > 0)
                    throw new DataExistException(DalText.Player_PlayerCodeExists.With(dao.PlayerCode));
            }

            // Update the player.
            player.PlayerCode = dao.PlayerCode;
            player.PlayerName = dao.PlayerName;

            int count = DbContext.SaveChanges();
            if (count == 0)
                throw new UpdateFailedException(DalText.Player_UpdateFailed.With(player.PlayerCode));

            // Return new data.
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// Deletes the specified player.
        /// </summary>
        /// <param name="criteria">The criteria of the player.</param>
        public void Delete(
            PlayerCriteria criteria
            )
        {
            int count = 0;

            // Get the specified player.
            var player = DbContext.Players
                .Where(e =>
                    e.PlayerKey == criteria.PlayerKey
                 )
                .AsNoTracking()
                .FirstOrDefault()
                ?? throw new DataNotFoundException(DalText.Player_NotFound);

            // Delete the player.
            DbContext.Players.Remove(player);

            count = DbContext.SaveChanges();
            if (count == 0)
                throw new DeleteFailedException(DalText.Player_DeleteFailed.With(player.PlayerCode));
        }

        #endregion Delete
    }
}
