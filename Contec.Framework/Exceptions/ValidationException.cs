using System;
using System.Linq;
using System.Collections.Generic;

using Contec.Framework.Errors;

namespace Contec.Framework.Exceptions
{
    public class ValidationException : Exception
    {
        private readonly ICollection<ValidationError> _errors;

        public ValidationException()
        {
            _errors = new List<ValidationError>();
        }

        public ValidationException(ValidationError e)
        {
            _errors = new List<ValidationError> { e };
        }

        public ValidationException(List<ValidationError> errors)
        {
            _errors = errors;
        }

        public ValidationException(string errorItem, string errorMessage, params object[] args)
        {
            if (args != null && args.Any())
            {
                errorMessage = string.Format(errorMessage, args);
            }

            _errors = new List<ValidationError> { new ValidationError(errorItem, errorMessage) };
        }

        public ValidationException(string message, ICollection<ValidationError> errors)
            : base(message)
        {

            _errors = errors;
        }

        public ValidationException(ICollection<ValidationError> errors, Exception innerException)
            : base(null, innerException)
        {

            _errors = errors;
        }

        public ValidationException(string message, ICollection<ValidationError> errors, Exception innerException)
            : base(message, innerException)
        {

            _errors = errors;
        }

        public ICollection<ValidationError> ValidationErrors
        {
            get { return _errors; }
        }
    }
}