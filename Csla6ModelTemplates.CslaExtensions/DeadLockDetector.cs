using Csla6ModelTemplates.Dal;
using System.Reflection;

namespace Csla6ModelTemplates.CslaExtensions
{
    public class DeadLockDetector : IDeadLockDetector
    {
        private readonly Dictionary<string, MethodInfo> Methods = new();

        public void RegisterCheckMethod(
            string dal,
            MethodInfo method
            )
        {
            Methods.Add(dal, method);
        }

        public DeadlockException CheckException(
            Exception exception
            )
        {
            Exception original = exception;
            while (original.InnerException != null)
                original = original.InnerException;

            DeadlockException result = null;

            foreach (var method in Methods)
            {
                bool isDeadLock = (bool)method.Value.Invoke(null, new object[] { original });
                if (isDeadLock)
                {
                    result = new DeadlockException(original.Message, exception);
                    break;
                }
            }
            return result;
        }
    }
}
