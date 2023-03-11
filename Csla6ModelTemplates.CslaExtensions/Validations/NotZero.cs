using Csla.Rules;
using Csla.Rules.CommonRules;
using Csla6ModelTemplates.Resources;

namespace Csla6ModelTemplates.CslaExtensions.Validations
{
    /// <summary>
    /// Business rule for a non-zero value.
    /// </summary>
    public class NotZero<T> : CommonBusinessRule
      where T : IComparable
    {
        private T Zero { get; set; }

        /// <summary>
        /// Gets or sets the format string used
        /// to format the Min value.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Creates an instance of the rule.
        /// </summary>
        /// <param name="primaryProperty">Property to which the rule applies.</param>
        public NotZero(Csla.Core.IPropertyInfo primaryProperty)
          : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }

        /// <summary>
        /// Creates an instance of the rule.
        /// </summary>
        /// <param name="primaryProperty">Property to which the rule applies.</param>
        /// <param name="message">The message.</param>
        public NotZero(Csla.Core.IPropertyInfo primaryProperty, string message)
          : this(primaryProperty)
        {
            MessageText = message;
        }

        /// <summary>
        /// Creates an instance of the rule.
        /// </summary>
        /// <param name="primaryProperty">Property to which the rule applies.</param>
        /// <param name="messageDelegate">The localizable message.</param>
        public NotZero(Csla.Core.IPropertyInfo primaryProperty, Func<string> messageDelegate)
          : this(primaryProperty)
        {
            MessageDelegate = messageDelegate;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value></value>
        protected override string GetMessage()
        {
            return HasMessageDelegate ? base.MessageText : CommonText.NotZeroRule_MessageText;
        }

        /// <summary>
        /// Rule implementation.
        /// </summary>
        /// <param name="context">Rule context.</param>
        protected override void Execute(IRuleContext context)
        {
            var value = context.InputPropertyValues[PrimaryProperty] != null
                            ? (T)context.InputPropertyValues[PrimaryProperty]
                            : PrimaryProperty.DefaultValue != null
                                ? (T)PrimaryProperty.DefaultValue
                                : default;

            var result = value.CompareTo(Zero);
            if (result == 0)
            {
                string outValue;
                if (string.IsNullOrEmpty(Format))
                    outValue = "0";
                else
                    outValue = string.Format(string.Format("{{0:{0}}}", Format), Zero);
                var message = string.Format(GetMessage(), PrimaryProperty.FriendlyName, outValue);
                context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
            }
        }
    }
}
