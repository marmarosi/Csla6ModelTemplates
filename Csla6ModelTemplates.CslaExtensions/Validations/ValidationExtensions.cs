using Csla6ModelTemplates.Resources;
using System.ComponentModel.DataAnnotations;

namespace Csla6ModelTemplates.CslaExtensions.Validations
{
    /// <summary>
    /// Provides method to set error message properties.
    /// </summary>
    internal static class ValidationExtensions
    {
        /// <summary>
        /// Sets the missing error message properties for the validation attribute.
        /// </summary>
        /// <param name="validation">The validation attribute.</param>
        /// <param name="context">The validation context.</param>
        /// <param name="ruleName">Name of the rule.</param>
        /// <exception cref="System.Exception">ValidationResourceTypeAttribute is required on the business object.</exception>
        public static void SetErrorMessage(
            this ValidationAttribute validation,
            ValidationContext context,
            string ruleName
            )
        {
            if (validation.ErrorMessageResourceType == null)
            {
                // Set the resource type.
                object[] attrs = context.ObjectType.GetCustomAttributes(
                    typeof(ValidationResourceTypeAttribute),
                    false
                    );
                if (attrs == null || attrs.Length == 0)
                    throw new Exception(CommonText.Validation_MissingAttribute);

                ValidationResourceTypeAttribute attr = attrs[0] as ValidationResourceTypeAttribute;
                validation.ErrorMessageResourceType = attr.ResourceType;

                // Set the resource name.
                if (string.IsNullOrWhiteSpace(validation.ErrorMessageResourceName))
                {
                    string format = "{0}_{1}_{2}";
                    string objectName = string.IsNullOrWhiteSpace(attr.ObjectName) ?
                        context.ObjectType.Name :
                        attr.ObjectName;
                    validation.ErrorMessageResourceName = format.With(objectName, context.MemberName, ruleName);
                }
            }
        }
    }
}
