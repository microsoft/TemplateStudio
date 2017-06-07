using System;

namespace Param_ItemNamespace.Models
{
    // TODO UWPTemplates: This is used by the Sample Grid Data. Remove this once your grid page is displaying real data
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
