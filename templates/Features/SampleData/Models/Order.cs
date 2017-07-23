using System;

namespace Param_ItemNamespace.Models
{
    // TODO WTS: This is used by the SampleDataService. Remove this once your pages/features are using your data
    // It is the model class we use to display data on pages like Grid, Chart, and Master Detail.
    public class Order
    {
        public long OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Company { get; set; }

        public string ShipTo { get; set; }

        public double OrderTotal { get; set; }

        public string Status { get; set; }
    }
}
