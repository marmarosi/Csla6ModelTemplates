using Csla6ModelTemplates.Contracts.Tree.Choice;
using Csla6ModelTemplates.Dal.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal.PostgreSql.Tree.Choice
{
    /// <summary>
    /// Implements the data access functions of the read-only tree choice collection.
    /// </summary>
    [DalImplementation]
    public class RootFolderChoiceDal : DalBase<PostgreSqlContext>, IRootFolderChoiceDal
    {
        #region Constructor

        /// <summary>
        /// Instantiates the data access object.
        /// </summary>
        /// <param name="context">The database context.</param>
        public RootFolderChoiceDal(
            PostgreSqlContext dbContext
            )
        {
            DbContext = dbContext;
        }

        #endregion Constructor

        #region Fetch

        /// <summary>
        /// Gets the choice of the trees.
        /// </summary>
        /// <param name="criteria">The criteria of the tree choice.</param>
        /// <returns>The data transfer object of the requested tree choice.</returns>
        public List<IdNameOptionDao> Fetch(
            RootFolderChoiceCriteria criteria
            )
        {
            var choice = DbContext.Folders
                .Where(e => e.ParentKey == null)
                .Select(e => new IdNameOptionDao
                {
                    Key = e.FolderKey,
                    Name = e.FolderName
                })
                .OrderBy(o => o.Name)
                .AsNoTracking()
                .ToList();

            return choice;
        }

        #endregion GetChoice
    }
}
