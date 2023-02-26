using System.Reflection;

namespace Csla6ModelTemplates.Dal.SqlServer
{
    public class SqlServerDalIndex : DalIndex
    {
        public SqlServerDalIndex()
        {
            DalAssembly = Assembly.GetExecutingAssembly();
        }
    }
}
