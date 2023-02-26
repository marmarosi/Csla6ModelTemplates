using System.ComponentModel.DataAnnotations;

namespace Csla6ModelTemplates.CslaExtensions.Validations
{
    /// <summary>
    /// Specifies that a data field value can be any integer but zero.
    /// </summary>
    public class NotZeroAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotZeroAttribute"/> class.
        /// </summary>
        public NotZeroAttribute()
            : base("^-?[1-9]\\d*$")
        { }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="context">The context information about the validation operation.</param>
        /// <returns><c>true</c> if value is valid; otherwise, <c>false</c>.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            // Set resource type and resource name for error message.
            this.SetErrorMessage(context, "NotZero");

            // Validate the value.
            return base.IsValid(value, context);
        }
    }
}
