//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace Csla6ModelTemplates.Dal
//{
//    /// <summary>
//    /// Defines the functionality of the manager object for a data access layer.
//    /// </summary>
//    public interface IDalManager : IDisposable
//    {
//        /// <summary>
//        /// Gets a data access object of the specified type.
//        /// </summary>
//        /// <typeparam name="T">The type of the data access object to instantiate.</typeparam>
//        /// <returns>The data access object of the specified type.</returns>
//        T GetProvider<T>() where T : class, IDal;

//        /// <summary>
//        /// Registers the data access layer constext as a service.
//        /// </summary>
//        /// <param name="configuration">The application configuration.</param>
//        /// <param name="services">The container of the application services.</param>
//        void AddDalContext(
//            IConfiguration configuration,
//            IServiceCollection services
//            );


//        /// <summary>
//        /// CHecks whether the reason of the exception is a deadlock.
//        /// </summary>
//        /// <param name="ex">The original exception thrown.</param>
//        /// <returns>True when the reason is a deadlock; otherwise false;</returns>
//        bool HasDeadlock(Exception ex);
//    }
//}
