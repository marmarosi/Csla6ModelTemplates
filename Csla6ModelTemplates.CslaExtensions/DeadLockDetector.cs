using Csla6ModelTemplates.Dal;
using System.Reflection;

namespace Csla6ModelTemplates.CslaExtensions
{
    /// <summary>
    /// Provides methods to identify database deadlocks.
    /// </summary>
    public class DeadLockDetector : IDeadLockDetector
    {
        private readonly Dictionary<string, MethodInfo> Methods = new();

        /// <summary>
        /// Registers a method to identify a database deadlock
        /// in the specified data access layer.
        /// </summary>
        /// <param name="dal">The name of the data access layer.</param>
        /// <param name="method">The deadlock detector method to call.</param>
        public void RegisterCheckMethod(
            string dal,
            MethodInfo method
            )
        {
            Methods.Add(dal, method);
        }

        /// <summary>
        /// Checks an exception whether it is a deadlock one.
        /// </summary>
        /// <param name="exception">The exception to check.</param>
        /// <returns>Returns true when the exception is a deadlock one; otherwise false.</returns>
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
