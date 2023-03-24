using Csla;
using Csla6ModelTemplates.Dal;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Defines the CSLA helper service.
    /// </summary>
    public interface ICslaService
    {
        /// <summary>
        /// Gets the CSLA data portal factory.
        /// </summary>
        public IDataPortalFactory Factory { get; }

        /// <summary>
        /// Gets the CSLA child data portal factory.
        /// </summary>
        public IChildDataPortalFactory ChildFactory { get; }

        /// <summary>
        /// Gets the deadlock detector service.
        /// </summary>
        public IDeadLockDetector DeadLock { get; }
    }
}
