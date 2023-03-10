namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Represents the properties of the database transactions.
    /// </summary>
    public class TransactionOptions : ITransactionOptions
    {
        /// <summary>
        /// Indicates whether the transaction is executed in an integration test.
        /// </summary>
        public bool IsTest { get; set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public TransactionOptions() { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="isTesting">True when the transaction runs in an integration test; otherwise false.</param>
        public TransactionOptions(
            bool isTesting
            )
        {
            IsTest = isTesting;
        }
    }
}
