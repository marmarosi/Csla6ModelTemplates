using System.Reflection;

namespace Csla6ModelTemplates.Dal.PostgreSql
{
    /// <summary>
    /// Represents a list of PostgreSQL data access implementations.
    /// </summary>
    public static class PostgreSqlDalIndex
    {
        /// <summary>
        /// Gets the list of data access implementations in the currwnt assembly.
        /// </summary>
        public static Dictionary<Type, Type> Items
        {
            get
            {
                var dalindex = new DalIndex(Assembly.GetExecutingAssembly());
                return dalindex.DalTypes;
            }
        }
    }
}
