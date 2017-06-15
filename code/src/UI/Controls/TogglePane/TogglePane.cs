using Microsoft.Templates.UI.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Templates.UI.Controls
{
    public sealed partial class TogglePane : Control
    {    
        private Grid _togglePaneShadowGrid;
        private Grid _menuGrid;
        private bool _isInitialized = false;

        static TogglePane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TogglePane), new FrameworkPropertyMetadata(typeof(TogglePane)));                        
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _togglePaneShadowGrid = GetTemplateChild("togglePaneShadowGrid") as Grid;
            _menuGrid = GetTemplateChild("menuGrid") as Grid;

            _isInitialized = true;
            UpdateOpenStatus();
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        private void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;            
        }

        private void UpdateOpenStatus(bool newValue = false, bool oldValue = false)
        {            
            if (_isInitialized)
            {
                if (IsOpen && newValue == false && oldValue == false)
                {
                    _menuGrid.AnimateWidth(153, 0);
                    _togglePaneShadowGrid.AnimateWidth(153,0);
                    _togglePaneShadowGrid.FadeIn(0);
                }
                else if (IsOpen)
                {
                    _menuGrid.AnimateWidth(153);
                    _togglePaneShadowGrid.AnimateWidth(153);
                    _togglePaneShadowGrid.FadeIn();
                }
                else
                {
                    _menuGrid.AnimateWidth(38);
                    _togglePaneShadowGrid.AnimateWidth(38);
                    _togglePaneShadowGrid.FadeOut();
                }
            }
        }
    }
}
