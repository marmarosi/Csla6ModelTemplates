using System;
using System.ComponentModel.DataAnnotations;

namespace Csla6ModelTemplates.CslaExtensions.Validations
{
    /// <summary>
    /// Specifies the numeric range constraints for the value of a data field.
    /// </summary>
    public class RangeAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class.
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public RangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class.
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public RangeAttribute(double minimum, double maximum)
            : base(minimum, maximum)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class.
        /// </summary>
        /// <param name="type">Specifies the type of the object to test.</param>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public RangeAttribute(Type type, string minimum, string maximum)
            : base(type, minimum, maximum)
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
            this.SetErrorMessage(context, "Range");

            // Validate the value.
            return base.IsValid(value, context);
        }
    }
}
