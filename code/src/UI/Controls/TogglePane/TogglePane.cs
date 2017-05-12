using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Input;

namespace Microsoft.Templates.UI.Controls
{
    public sealed partial class TogglePane : Control
    {
        private ContentPresenter _mainViewContentTemplate;
        private Grid _shadowGrid;
        private Grid _secondaryShadowGrid;
        private Grid _fakeGrid;
        private bool _isInitialized = false;

        static TogglePane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TogglePane), new FrameworkPropertyMetadata(typeof(TogglePane)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainViewContentTemplate = GetTemplateChild("mainViewContentTemplate") as ContentPresenter;
            _shadowGrid = GetTemplateChild("shadowGrid") as Grid;
            _secondaryShadowGrid = GetTemplateChild("secondaryShadowGrid") as Grid;
            _fakeGrid = GetTemplateChild("fakeGrid") as Grid;
            SizeChanged += (sender, e) => Initialize();
            MouseEnter += (sender, e) => MouseEnterAction?.Invoke();
            MouseLeave += (sender, e) => MouseLeaveAction?.Invoke();
            _isInitialized = true;
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                _fakeGrid.Width = this.ActualWidth - 30;
            }
        }

        private void UpdateOpenStatus()
        {
            if (_isInitialized)
            {
                if (IsOpen)
                {
                    _fakeGrid.AnimateWidth(30);
                    _secondaryShadowGrid.FadeIn();
                }
                else
                {
                    _fakeGrid.AnimateWidth(ActualWidth - 30);
                    _secondaryShadowGrid.FadeOut();
                }
            }
        }
    }
}
