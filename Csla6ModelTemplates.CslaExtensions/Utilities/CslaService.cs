using Csla;
using Csla6ModelTemplates.Dal;

namespace Csla6ModelTemplates.CslaExtensions.Utilities
{
    /// <summary>
    /// Provides services for business objects.
    /// </summary>
    public class CslaService : ICslaService
    {
        /// <summary>
        /// Gets the CSLA data portal factory.
        /// </summary>
        public IDataPortalFactory Factory { get; private set; }

        /// <summary>
        /// Gets the CSLA child data portal factory.
        /// </summary>
        public IChildDataPortalFactory ChildFactory { get; private set; }

        /// <summary>
        /// Gets the deadlock detector service.
        /// </summary>
        public IDeadLockDetector DeadLock { get; private set; }


        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="factory">The CSLA data portal factory.</param>
        /// <param name="childFactory">The CSLA child data portal factory.</param>
        /// <param name="detector">The deadlock detector service.</param>
        public CslaService(
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory,
            IDeadLockDetector detector)
        {
            Factory = factory;
            ChildFactory = childFactory;
            DeadLock = detector;
        }
    }
}
