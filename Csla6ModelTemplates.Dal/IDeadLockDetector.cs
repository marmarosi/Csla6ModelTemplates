using System.Reflection;

namespace Csla6ModelTemplates.Dal
{
    public interface IDeadLockDetector
    {
        public void RegisterCheckMethod(
            string dal,
            MethodInfo method
            );

        public DeadlockException CheckException(
            Exception exception
            );
    }
}
