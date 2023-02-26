namespace Csla6ModelTemplates.Dal.SqlServer.Simple
{
    /// <summary>
    /// Implements the data access functions of the simple models.
    /// </summary>
    [DalImplementation]
    public partial class SimpleDal
    {
        private readonly SqlServerContext Context;

        public SimpleDal(
            SqlServerContext context
            )
        {
            Context = context;
        }
    }
}
