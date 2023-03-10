using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Implements the functionality of a data access layer implementation.
    /// </summary>
    public class DalBase<T> : ITransactionalDal where T : DbContext, ITransactionOptions
    {
        /// <summary>
        /// The database context.
        /// </summary>
        public T DbContext { get; protected set; }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <returns>The database transaction.</returns>
        public IDbContextTransaction BeginTransaction()
        {
            return DbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// Commits the specified transaction when it is not executed in integration test.
        /// </summary>
        /// <param name="transaction">The current database transaction to commit.</param>
        public void Commit(
            IDbContextTransaction transaction
            )
        {
            if (!DbContext.IsTest)
                transaction.Commit();
        }
    }
}
