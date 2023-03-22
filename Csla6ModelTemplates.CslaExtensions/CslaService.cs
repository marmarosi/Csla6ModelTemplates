using Csla;
using Csla6ModelTemplates.Dal;

namespace Csla6ModelTemplates.CslaExtensions
{
    public class CslaService : ICslaService
    {
        public IDataPortalFactory Factory { get; private set; }
        public IChildDataPortalFactory ChildFactory { get; private set; }
        public IDeadLockDetector DeadLock { get; private set; }

        public CslaService(
            IDataPortalFactory factory,
            IChildDataPortalFactory childFactory,
            IDeadLockDetector detector            )
        {
            Factory = factory;
            ChildFactory = childFactory;
            DeadLock = detector;
        }
    }
}
