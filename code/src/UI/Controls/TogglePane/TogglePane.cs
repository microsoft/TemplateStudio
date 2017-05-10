using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;
using System.Windows;
using System.Windows.Controls;
using System;

namespace Microsoft.Templates.UI.Controls
{
    public sealed partial class TogglePane : Control
    {
        private ContentPresenter _mainViewContentTemplate;
        private bool _isInitialized = false;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainViewContentTemplate = GetTemplateChild("mainViewContentTemplate") as ContentPresenter;
            SizeChanged += (sender, e) => Initialize();
            _isInitialized = true;
        }

        static TogglePane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TogglePane), new FrameworkPropertyMetadata(typeof(TogglePane)));
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                _mainViewContentTemplate.Width = this.ActualWidth - 30;
            }
        }

        private void UpdateOpenStatus()
        {
            if (_isInitialized)
            {
                if (IsOpen)
                {
                    _mainViewContentTemplate.AnimateWidth(30);
                }
                else
                {
                    _mainViewContentTemplate.AnimateWidth(ActualWidth - 30);
                }
            }
        }
    }
}
