using System.Reflection;

namespace Csla6ModelTemplates.Dal.SqlServer
{
    public static class SqlServerDalIndex
    {
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
