using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Xaml.Interactivity;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Param_RootNamespace.Behaviors
{
    public class TreeViewExpandCollapseBehavior : Behavior<WinUI.TreeView>
    {
        public ICommand ExpandAllCommand { get; }

        public ICommand CollapseAllCommand { get; }

        public TreeViewExpandCollapseBehavior()
        {
            CollapseAllCommand = new RelayCommand(() => ExpandOrCollapse(AssociatedObject.RootNodes, false));
            ExpandAllCommand = new RelayCommand(() => ExpandOrCollapse(AssociatedObject.RootNodes, true));
        }


        private void ExpandOrCollapse(IList<WinUI.TreeViewNode> nodes, bool expand)
        {
            foreach (var node in nodes)
            {
                ExpandOrCollapse(node.Children, expand);

                if (expand)
                {
                    AssociatedObject.Expand(node);
                }
                else
                {
                    AssociatedObject.Collapse(node);
                }
            }
        }
    }
}
