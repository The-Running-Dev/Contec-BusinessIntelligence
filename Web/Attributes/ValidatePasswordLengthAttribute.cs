using System;
using System.Web.Security;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace BI.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;

            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
}
