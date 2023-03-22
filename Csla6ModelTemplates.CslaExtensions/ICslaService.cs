using Csla;
using Csla6ModelTemplates.Dal;

namespace Csla6ModelTemplates.CslaExtensions
{
    public interface ICslaService
    {
        public IDataPortalFactory Factory { get; }
        public IChildDataPortalFactory ChildFactory { get; }
        public IDeadLockDetector DeadLock { get; }
    }
}
