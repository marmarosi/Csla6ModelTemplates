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
        public bool IsUnderTest { get; set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public TransactionOptions() { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="isUnderTest">True when the transaction runs in an integration test; otherwise false.</param>
        public TransactionOptions(
            bool isUnderTest
            )
        {
            IsUnderTest = isUnderTest;
        }
    }
}
