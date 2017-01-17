using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroApp.Dessert.Detail
{
    public class DessertDetailViewModel : Screen
    {
        private DessertModel _parameter;
        public DessertModel Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                NotifyOfPropertyChange(() => Parameter);
            }
        }

        protected override void OnActivate()
        {

        }
    }
}
