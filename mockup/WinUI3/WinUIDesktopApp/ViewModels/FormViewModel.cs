using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using WinUIDesktopApp.Core.Contracts.Services;
using WinUIDesktopApp.Core.Models;
using WinUIDesktopApp.Helpers;

namespace WinUIDesktopApp.ViewModels
{
    public class FormViewModel : ValidationObservableRecipient
    {
        private readonly ISampleDataService _sampleDataService;

        private long _orderID;
        private DateTimeOffset _orderDate = DateTime.Now;
        private TimeSpan _orderTime = DateTime.Now.TimeOfDay;
        private string _company;
        private char _symbol;
        private string _orderTotal;
        private string _freight;
        private string _status;
        private string _shipperName;
        private string _shipperPhone;
        private string _shipTo;
        private ICommand _submitCommand;

        [Required]
        [Range(10000, 99999)]
        public long OrderID
        {
            get { return _orderID; }
            set { SetAndValidate(ref _orderID, value); }
        }

        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset OrderDate
        {
            get { return _orderDate; }
            set { SetAndValidate(ref _orderDate, value); }
        }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan OrderTime
        {
            get { return _orderTime; }
            set { SetAndValidate(ref _orderTime, value); }
        }

        [Required]
        public string Company
        {
            get { return _company; }
            set { SetAndValidate(ref _company, value); }
        }

        [Required]
        public char Symbol
        {
            get { return _symbol; }
            set { SetAndValidate(ref _symbol, value); }
        }

        [Required]
        [CustomValidation(typeof(FormViewModel), "ValidateDoubleProperty")]
        public string OrderTotal
        {
            get { return _orderTotal; }
            set { SetAndValidate(ref _orderTotal, value); }
        }

        [Required]
        [CustomValidation(typeof(FormViewModel), "ValidateDoubleProperty")]
        public string Freight
        {
            get { return _freight; }
            set { SetAndValidate(ref _freight, value); }
        }

        [Required]
        public string Status
        {
            get { return _status; }
            set { SetAndValidate(ref _status, value); }
        }        

        [Required]
        public string ShipperName
        {
            get { return _shipperName; }
            set { SetAndValidate(ref _shipperName, value); }
        }

        [Required]
        [Phone]
        public string ShipperPhone
        {
            get { return _shipperPhone; }
            set { SetAndValidate(ref _shipperPhone, value); }
        }        

        [Required]
        public string ShipTo
        {
            get { return _shipTo; }
            set { SetAndValidate(ref _shipTo, value); }
        }        

        public IEnumerable<string> StatusValues { get; } = new List<string>()
        {
            "New",
            "Shipped",
            "Closed"
        };

        public IEnumerable<char> SymbolValues { get; } = new List<char>()
        {
            (char)57643,
            (char)57737,
            (char)57699,
            (char)57620,
            (char)57633,
            (char)57661,
            (char)57619,
            (char)57615
        };

        public ICommand SubmitCommand => _submitCommand ?? (_submitCommand = new RelayCommand(Submit));

        public FormViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
            Status = StatusValues.First();
            Symbol = SymbolValues.First();
        }

        private async void Submit()
        {
            var hasValidationErrors = ValidateProperties(new Dictionary<string, object>()
            {
                { nameof(OrderID), OrderID},                
                { nameof(OrderDate), OrderDate},
                { nameof(OrderTime), OrderTime},
                { nameof(Company), Company },
                { nameof(Symbol), Symbol},
                { nameof(OrderTotal), OrderTotal},
                { nameof(Freight), Freight},
                { nameof(Status), Status},
                { nameof(ShipperName), ShipperName },
                { nameof(ShipperPhone), ShipperPhone },                
                { nameof(ShipTo), ShipTo }
            });

            if (hasValidationErrors)
            {
                return;
            }

            await _sampleDataService.SaveOrderAsync(new SampleOrder()
            {
                OrderID = OrderID,
                OrderDate = new DateTime(OrderDate.Year, OrderDate.Month, OrderDate.Day, OrderTime.Hours, OrderTime.Minutes, OrderTime.Seconds),
                ShipperName = ShipperName,
                ShipperPhone = ShipperPhone,
                Company = Company,
                ShipTo = ShipTo,
                OrderTotal = double.Parse(OrderTotal),
                Status = Status,
                Freight = double.Parse(Freight),
                SymbolCode = Symbol
            });

            // Set default values
            OrderID = default;
            OrderDate = DateTime.Now;
            OrderTime = DateTime.Now.TimeOfDay;
            ShipperName = string.Empty;
            ShipperPhone = string.Empty;
            Company = string.Empty;
            ShipTo = string.Empty;
            OrderTotal = default;
            Freight = default;
            Status = StatusValues.First();
            Symbol = SymbolValues.First();
            ClearErrors();
            //var dialog = new ContentDialog()
            //{
            //    Title = "New Order",
            //    Content = "Order submitted successfully",
            //    CloseButtonText = "Ok"
            //};

            //await dialog.ShowAsync();
        }

        public static ValidationResult ValidateDoubleProperty(string property)
            => double.TryParse(property, out var result) ? ValidationResult.Success : new ValidationResult("Double property required");
    }
}
