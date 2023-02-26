using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Represents a session with tha database.
    /// </summary>
    public class DbContextBase : DbContext
    {
        public string ConnectionString { get; protected set; }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public DbContextBase()
        { }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="dalName">The name of the data access layer.</param>
        public DbContextBase(
            string dalName
            )
        {
            ConnectionString = DalFactory.GetConnectionString(dalName);
        }
    }
}
