using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using WinUIDesktopApp.Core.Contracts.Services;
using WinUIDesktopApp.Core.Models;

namespace WinUIDesktopApp.ViewModels
{
    public class FormWCTViewModel : ObservableValidator
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
            set { SetProperty(ref _orderID, value, true); }
        }

        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset OrderDate
        {
            get { return _orderDate; }
            set { SetProperty(ref _orderDate, value, true); }
        }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan OrderTime
        {
            get { return _orderTime; }
            set { SetProperty(ref _orderTime, value, true); }
        }

        [Required]
        public string Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value, true); }
        }

        [Required]
        public char Symbol
        {
            get { return _symbol; }
            set { SetProperty(ref _symbol, value, true); }
        }

        [Required]
        [CustomValidation(typeof(FormWCTViewModel), "ValidateDoubleProperty")]
        public string OrderTotal
        {
            get { return _orderTotal; }
            set { SetProperty(ref _orderTotal, value, true); }
        }

        [Required]
        [CustomValidation(typeof(FormWCTViewModel), "ValidateDoubleProperty")]
        public string Freight
        {
            get { return _freight; }
            set { SetProperty(ref _freight, value, true); }
        }

        [Required]
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value, true); }
        }

        [Required]
        public string ShipperName
        {
            get { return _shipperName; }
            set { SetProperty(ref _shipperName, value, true); }
        }

        [Required]
        [Phone]
        public string ShipperPhone
        {
            get { return _shipperPhone; }
            set { SetProperty(ref _shipperPhone, value, true); }
        }

        [Required]
        public string ShipTo
        {
            get { return _shipTo; }
            set { SetProperty(ref _shipTo, value, true); }
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

        public FormWCTViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
            Status = StatusValues.First();
            Symbol = SymbolValues.First();
        }

        private async void Submit()
        {
            // TODO: Validate property errors in all of these properties
            // OrderID
            // OrderDate
            // OrderTime
            // Company
            // Symbol
            // OrderTotal
            // Freight
            // Status
            // ShipperName
            // ShipperPhone
            // ShipTo
            if (HasErrors)
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

            // TODO: Set default values and clean all erros in all of these properties
            // OrderID
            // OrderDate
            // OrderTime
            // Company
            // Symbol
            // OrderTotal
            // Freight
            // Status
            // ShipperName
            // ShipperPhone
            // ShipTo

            // TODO Clean all erros
        }

        public static ValidationResult ValidateDoubleProperty(string property)
            => double.TryParse(property, out var result) ? ValidationResult.Success : new ValidationResult("Double property required");
    }
}
