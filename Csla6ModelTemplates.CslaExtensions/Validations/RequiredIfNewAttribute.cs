using Csla.Core;
using System.ComponentModel.DataAnnotations;

namespace Csla6ModelTemplates.CslaExtensions.Validations
{
    /// <summary>
    /// Specifies that a data field value is required only when the business object is new.
    /// </summary>
    public class RequiredIfNewAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfNewAttribute"/> class.
        /// </summary>
        public RequiredIfNewAttribute()
            : base()
        { }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="context">The context information about the validation operation.</param>
        /// <returns><c>true</c> if value is valid; otherwise, <c>false</c>.</returns>
        protected override ValidationResult IsValid(
            object value,
            ValidationContext context
            )
        {
            // Set resource type and resource name for error message.
            this.SetErrorMessage(context, "RequiredIfNew");

            // Validate the value.
            BusinessBase model = (BusinessBase)context.ObjectInstance;
            if (model.IsNew)
                return base.IsValid(value, context);
            else
                return null;
        }
    }
}
