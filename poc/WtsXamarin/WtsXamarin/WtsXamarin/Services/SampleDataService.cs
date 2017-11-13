using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WtsXamarin.Models;

namespace WtsXamarin.Services
{
    public static class SampleDataService
    {
        private static IEnumerable<SampleOrder> AllOrders()
        {
            // The following is order summary data
            var data = new ObservableCollection<SampleOrder>
            {
                new SampleOrder
                {
                    OrderId = 70,
                    OrderDate = new DateTime(2017, 05, 24),
                    Company = "Company F",
                    ShipTo = "Francisco Pérez-Olaeta",
                    OrderTotal = 2490.00,
                    Status = "Closed"
                },
                new SampleOrder
                {
                    OrderId = 71,
                    OrderDate = new DateTime(2017, 05, 24),
                    Company = "Company CC",
                    ShipTo = "Soo Jung Lee",
                    OrderTotal = 1760.00,
                    Status = "Closed"
                },
                new SampleOrder
                {
                    OrderId = 72,
                    OrderDate = new DateTime(2017, 06, 03),
                    Company = "Company Z",
                    ShipTo = "Run Liu",
                    OrderTotal = 2310.00,
                    Status = "Closed"
                },
                new SampleOrder
                {
                    OrderId = 73,
                    OrderDate = new DateTime(2017, 06, 05),
                    Company = "Company Y",
                    ShipTo = "John Rodman",
                    OrderTotal = 665.00,
                    Status = "Closed"
                },
                new SampleOrder
                {
                    OrderId = 74,
                    OrderDate = new DateTime(2017, 06, 07),
                    Company = "Company H",
                    ShipTo = "Elizabeth Andersen",
                    OrderTotal = 560.00,
                    Status = "Shipped"
                },
                new SampleOrder
                {
                    OrderId = 75,
                    OrderDate = new DateTime(2017, 06, 07),
                    Company = "Company F",
                    ShipTo = "Francisco Pérez-Olaeta",
                    OrderTotal = 810.00,
                    Status = "Shipped"
                },
                new SampleOrder
                {
                    OrderId = 76,
                    OrderDate = new DateTime(2017, 06, 11),
                    Company = "Company I",
                    ShipTo = "Sven Mortensen",
                    OrderTotal = 196.50,
                    Status = "Shipped"
                },
                new SampleOrder
                {
                    OrderId = 77,
                    OrderDate = new DateTime(2017, 06, 14),
                    Company = "Company BB",
                    ShipTo = "Amritansh Raghav",
                    OrderTotal = 270.00,
                    Status = "New"
                },
                new SampleOrder
                {
                    OrderId = 78,
                    OrderDate = new DateTime(2017, 06, 14),
                    Company = "Company A",
                    ShipTo = "Anna Bedecs",
                    OrderTotal = 736.00,
                    Status = "New"
                },
                new SampleOrder
                {
                    OrderId = 79,
                    OrderDate = new DateTime(2017, 06, 18),
                    Company = "Company K",
                    ShipTo = "Peter Krschne",
                    OrderTotal = 800.00,
                    Status = "New"
                },
            };

            return data;
        }

        // TODO WTS: Remove this once your grid page is displaying real data
        public static ObservableCollection<SampleOrder> GetGridSampleData()
        {
            return new ObservableCollection<SampleOrder>(AllOrders());
        }

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<SampleOrder>> GetSampleModelDataAsync()
        {
            await Task.CompletedTask;

            return AllOrders();
        }
    }
}
