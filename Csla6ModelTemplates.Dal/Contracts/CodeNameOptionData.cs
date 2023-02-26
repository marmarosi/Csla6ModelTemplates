namespace Csla6ModelTemplates.Dal.Contracts
{
    /// <summary>
    /// Defines the read-only code-name option data.
    /// </summary>
    public class CodeNameOptionData
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Defines the data access object of the read-only key-name option object.
    /// </summary>
    public class CodeNameOptionDao : CodeNameOptionData
    { }

    /// <summary>
    /// Defines the data transfer object of the read-only key-name option object.
    /// </summary>
    public class CodeNameOptionDto : CodeNameOptionData
    { }
}
