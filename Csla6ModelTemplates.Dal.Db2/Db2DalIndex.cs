using System.Reflection;

namespace Csla6ModelTemplates.Dal.Db2
{
    /// <summary>
    /// Represents a list of DB2 data access implementations.
    /// </summary>
    public static class Db2DalIndex
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
