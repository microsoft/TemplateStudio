using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DragAndDropExample.Services
{
    class ListViewDropConfiguration : DropConfiguration
    {
        public ICommand OnDragItemsStartingCommand { get; set; }
    }
}
