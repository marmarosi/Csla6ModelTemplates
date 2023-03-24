using System.Reflection;

namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Defines a service to identify database deadlocks.
    /// </summary>
    public interface IDeadLockDetector
    {
        /// <summary>
        /// Registers a method to identify a database deadlock
        /// in the specified data access layer.
        /// </summary>
        /// <param name="dal">The name of the data access layer.</param>
        /// <param name="method">The deadlock detector method to call.</param>
        public void RegisterCheckMethod(
            string dal,
            MethodInfo method
            );

        /// <summary>
        /// Checks an exception whether it is a deadlock one.
        /// </summary>
        /// <param name="exception">The exception to check.</param>
        /// <returns>Returns true when the exception is a deadlock one; otherwise false.</returns>
        public DeadlockException CheckException(
            Exception exception
            );
    }
}
