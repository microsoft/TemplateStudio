using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Data;

namespace Param_RootNamespace.Helpers
{
    public class ObservableValidator : ObservableRecipient, INotifyDataErrorInfo
    {
        private Dictionary<string, List<ValidationResult>> _errors = new Dictionary<string, List<ValidationResult>>();

        public bool HasErrors => _errors.Any();

        public IEnumerable<object> GetErrors(string propertyName)
            => _errors[propertyName];

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void SetAndValidate<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            var result = SetProperty(ref currentValue, newValue, propertyName);
            if (result)
            {
                ValidateProperty(propertyName, newValue);
            }
        }

        protected bool ValidateProperties(Dictionary<string, object> properties)
        {
            bool hasValidationErrors = false;
            foreach (var property in properties)
            {
                if (ValidateProperty(property.Key, property.Value).Any())
                {
                    hasValidationErrors = true;
                }
            }

            return hasValidationErrors;
        }

        protected void ClearErrors()
        {
            foreach (var error in _errors)
            {
                ClearErrors(error.Key);
            }
        }

        private IEnumerable<ValidationResult> ValidateProperty(string propertyName, object newValue)
        {
            ClearErrors(propertyName);
            var validationResults = new List<ValidationResult>();
            var validationResult = Validator.TryValidateProperty(newValue, new ValidationContext(this, null, null) { MemberName = propertyName }, validationResults);

            if (!validationResult)
            {
                AddErrors(propertyName, validationResults);
            }

            return validationResults;
        }

        private void AddErrors(string propertyName, IEnumerable<ValidationResult> validationResults)
        {
            List<ValidationResult> errors = null;
            if (!_errors.TryGetValue(propertyName, out errors))
            {
                errors = new List<ValidationResult>();
                _errors.Add(propertyName, errors);
            }

            errors.AddRange(validationResults);
            ErrorsChanged?.Invoke(this, new Microsoft.UI.Xaml.Data.DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.TryGetValue(propertyName, out var properyErrors))
            {
                properyErrors.Clear();
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}
