using Csla.Rules;

namespace Csla6ModelTemplates.CslaExtensions.Validations
{
    /// <summary>
    /// Information about the failed validation to send to the client.
    /// </summary>
    [Serializable]
    public class ValidationMessage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the business object model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the name of the business object property.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the description of the failed validation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the severity of the failed validation.
        /// </summary>
        public RuleSeverity Severity { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="model">The name of the business object model.</param>
        /// <param name="propertyPrefix">The prefix of the property name.</param>
        /// <param name="brokenRule">The broken rule.</param>
        public ValidationMessage(
            string model,
            string propertyPrefix,
            BrokenRule brokenRule
            )
        {
            Model = model;
            Property = propertyPrefix + brokenRule.Property;
            Description = brokenRule.Description;
            Severity = brokenRule.Severity;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="model">The name of the business object model.</param>
        /// <param name="property">The name of the proerty with optiona prefix.</param>
        /// <param name="description">The message text.</param>
        /// <param name="severity">The rule severity, defaults to error.</param>
        public ValidationMessage(
            string model,
            string property,
            string description,
            RuleSeverity severity = RuleSeverity.Error
            )
        {
            Model = model;
            Property = property;
            Description = description;
            Severity = severity;
        }

        #endregion
    }
}
