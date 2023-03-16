using System.Reflection;

namespace Csla6ModelTemplates.Dal.SqlServer
{
    /// <summary>
    /// Represents a list of SQL Server data access implementations.
    /// </summary>
    public static class SqlServerDalIndex
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
