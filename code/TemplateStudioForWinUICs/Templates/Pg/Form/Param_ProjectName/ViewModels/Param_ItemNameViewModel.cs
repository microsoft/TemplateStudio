using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.ViewModels
{
    public class Param_ItemNameViewModel : ObservableValidator
    {
        private readonly ISampleDataService _sampleDataService;

        private long _orderID;
        private DateTimeOffset _orderDate = DateTime.Now;
        private TimeSpan _orderTime = DateTime.Now.TimeOfDay;
        private string _company;
        private SampleSymbol _symbol;
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
            get => _orderID;
            set => SetProperty(ref _orderID, value, true);
        }

        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset OrderDate
        {
            get => _orderDate;
            set => SetProperty(ref _orderDate, value, true);
        }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan OrderTime
        {
            get => _orderTime;
            set => SetProperty(ref _orderTime, value, true);
        }

        [Required]
        public string Company
        {
            get => _company;
            set => SetProperty(ref _company, value, true);
        }

        [Required]
        public SampleSymbol Symbol
        {
            get => _symbol;
            set => SetProperty(ref _symbol, value, true);
        }

        [Required]
        [CustomValidation(typeof(Param_ItemNameViewModel), "ValidateDoubleProperty")]
        public string OrderTotal
        {
            get => _orderTotal;
            set => SetProperty(ref _orderTotal, value, true);
        }

        [Required]
        [CustomValidation(typeof(Param_ItemNameViewModel), "ValidateDoubleProperty")]
        public string Freight
        {
            get => _freight;
            set => SetProperty(ref _freight, value, true);
        }

        [Required]
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value, true);
        }

        [Required]
        public string ShipperName
        {
            get => _shipperName;
            set => SetProperty(ref _shipperName, value, true);
        }

        [Required]
        [Phone]
        public string ShipperPhone
        {
            get => _shipperPhone;
            set => SetProperty(ref _shipperPhone, value, true);
        }

        [Required]
        public string ShipTo
        {
            get => _shipTo;
            set => SetProperty(ref _shipTo, value, true);
        }

        public IEnumerable<string> StatusValues { get; } = new List<string>()
        {
            "New",
            "Shipped",
            "Closed"
        };

        public IEnumerable<SampleSymbol> SymbolValues { get; } = new List<SampleSymbol>
        {
            new SampleSymbol() { Name = "Globe", Code = (char)57643 },
            new SampleSymbol() { Name = "Audio", Code = (char)57737 },
            new SampleSymbol() { Name = "Calendar", Code = (char)57699 },
            new SampleSymbol() { Name = "Camera", Code = (char)57620 },
            new SampleSymbol() { Name = "Clock", Code = (char)57633 },
            new SampleSymbol() { Name = "Contact", Code = (char)57661 },
            new SampleSymbol() { Name = "Favorite", Code = (char)57619 },
            new SampleSymbol() { Name = "Home", Code = (char)57615 },
        };

        public ICommand SubmitCommand => _submitCommand ?? (_submitCommand = new RelayCommand(Submit));

        public Param_ItemNameViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
            Status = StatusValues.First();
            Symbol = SymbolValues.First();
        }

        private async void Submit()
        {
            ValidateAllProperties();

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
                SymbolCode = Symbol.Code,
                SymbolName = Symbol.Name,
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
        }

        public static ValidationResult ValidateDoubleProperty(string property) =>
            double.TryParse(property, out var result) ? ValidationResult.Success : new ValidationResult("Double property required");
    }
}
