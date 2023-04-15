using System.Reflection;

namespace Csla6ModelTemplates.Dal.Oracle
{
    /// <summary>
    /// Represents a list of Oracle data access implementations.
    /// </summary>
    public static class OracleDalIndex
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
