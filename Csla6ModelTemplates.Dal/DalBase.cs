using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Implements the functionality of a data access layer.
    /// </summary>
    public class DalBase<T> where T :
        IInfrastructure<IServiceProvider>,
        IDbContextDependencies,
        IDbSetCache,
        IDbContextPoolable
    {
        public T DbContext { get; protected set; }
    }
}
