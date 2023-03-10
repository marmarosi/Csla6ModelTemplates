using Microsoft.EntityFrameworkCore.Storage;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Defines the transaction functionality of a data access layer implementation.
    /// </summary>
    public interface ITransactionalDal
    {
        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <returns>The database transaction.</returns>
        public IDbContextTransaction BeginTransaction();

        /// <summary>
        /// Commits the specified transaction when it is not executed in integration test.
        /// </summary>
        /// <param name="transaction">The current database transaction to commit.</param>
        public void Commit(IDbContextTransaction transaction);
    }
}
