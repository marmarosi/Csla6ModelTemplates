using System.ComponentModel.DataAnnotations;

namespace Csla6ModelTemplates.CslaExtensions.Validations
{
    /// <summary>
    /// Specifies that a data field value must match the specified regular expression.
    /// </summary>
    public class PatternAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternAttribute"/> class.
        /// </summary>
        /// <param name="pattern">The regular expression that is used to validate the data field value.</param>
        public PatternAttribute(string pattern)
            : base(pattern)
        { }

        /// <summary>
        /// Gets or sets the suffix of the resource name.
        /// </summary>
        /// <value>
        /// The suffix of the resource name.
        /// </value>
        public string Suffix { get; set; }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="context">The context information about the validation operation.</param>
        /// <returns><c>true</c> if value is valid; otherwise, <c>false</c>.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            // Set resource type and resource name for error message.
            string nameSuffix = string.IsNullOrWhiteSpace(Suffix) ? "Pattern" : Suffix;
            this.SetErrorMessage(context, nameSuffix);

            // Validate the value.
            return base.IsValid(value, context);
        }
    }
}
