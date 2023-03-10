namespace Csla6ModelTemplates.Dal
{
    /// <summary>
    /// Definres the properties of the database transactions.
    /// </summary>
    public interface ITransactionOptions
    {
        /// <summary>
        /// Indicates whether the transaction is executed in an integration test.
        /// </summary>
        public bool IsTest { get; }
    }
}
