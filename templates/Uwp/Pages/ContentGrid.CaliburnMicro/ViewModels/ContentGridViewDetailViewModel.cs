using System;
using System.Linq;
using Caliburn.Micro;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.Core.Services;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class ContentGridViewDetailViewModel : Screen
    {
        private readonly IConnectedAnimationService _connectedAnimationService;

        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public void Initialize(long orderId)
        {
            // TODO WTS: Replace this with your actual data
            var data = SampleDataService.GetContentGridData();
            Item = data.First(i => i.OrderId == orderId);
        }

        public void SetListDataItemForNextConnectedAnnimation()
        {
            _connectedAnimationService.SetListDataItemForNextConnectedAnnimation(Item);
        }
    }
}
