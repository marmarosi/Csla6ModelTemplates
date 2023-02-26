using Microsoft.EntityFrameworkCore;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Defines the functionality of a data access layer.
    /// </summary>
    public interface IDal
    {
        DbContext DbContext { get; set; }
    }
}
