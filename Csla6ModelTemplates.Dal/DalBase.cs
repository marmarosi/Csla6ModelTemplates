using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Implements the functionality of a data access layer.
    /// </summary>
    public class DalBase : IDal
    {
        public DbContext DbContext { get; set; }
    }
}
