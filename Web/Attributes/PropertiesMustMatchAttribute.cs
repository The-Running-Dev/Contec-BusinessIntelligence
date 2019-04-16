using System;
using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BI.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(DefaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public override object TypeId { get; } = new object();

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            var properties = TypeDescriptor.GetProperties(value);
            var originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            var confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);

            return Equals(originalValue, confirmValue);
        }

        private const string DefaultErrorMessage = "'{0}' and '{1}' do not match.";

        private string ConfirmProperty { get; }

        private string OriginalProperty { get; }
    }
}