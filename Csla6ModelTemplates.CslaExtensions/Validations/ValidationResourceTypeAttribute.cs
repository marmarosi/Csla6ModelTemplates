using System;

namespace Csla6ModelTemplates.CslaExtensions.Validations
{
    /// <summary>
    /// Defines the resource type of the validation messages.
    /// </summary>
    public class ValidationResourceTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResourceTypeAttribute"/> class.
        /// </summary>
        /// <param name="type">The resource type of the validation messages.</param>
        /// <exception cref="System.ArgumentNullException">Argument type is required.</exception>
        public ValidationResourceTypeAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            ResourceType = type;
        }

        /// <summary>
        /// Gets the resource type of the validation messages.
        /// </summary>
        /// <value>
        /// The  resource type of the validation messages.
        /// </value>
        public Type ResourceType { get; private set; }

        /// <summary>
        /// Gets or sets the alias name of the object.
        /// </summary>
        /// <value>
        /// The alias name of the object.
        /// </value>
        public string ObjectName { get; set; }
    }
}
