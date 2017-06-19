// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.Controls
{
    public sealed partial class TogglePane : Control
    {
        private Border _togglePaneShadowGrid;
        private Grid _menuGrid;
        private bool _isInitialized = false;

        static TogglePane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TogglePane), new FrameworkPropertyMetadata(typeof(TogglePane)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _togglePaneShadowGrid = GetTemplateChild("togglePaneShadowGrid") as Border;
            _menuGrid = GetTemplateChild("menuGrid") as Grid;

            _isInitialized = true;
            UpdateOpenStatus();
        }

        private void UpdateOpenStatus(bool newValue = false, bool oldValue = false)
        {
            if (_isInitialized)
            {
                if (IsOpen && newValue == false && oldValue == false)
                {
                    _menuGrid.AnimateWidth(90, 0);
                    _togglePaneShadowGrid.AnimateWidth(90, 0);
                    _togglePaneShadowGrid.FadeIn(0);
                }
                else if (IsOpen)
                {
                    _menuGrid.AnimateWidth(90);
                    _togglePaneShadowGrid.AnimateWidth(90);
                    _togglePaneShadowGrid.FadeIn();
                }
                else
                {
                    _menuGrid.AnimateWidth(30);
                    _togglePaneShadowGrid.AnimateWidth(30);
                    _togglePaneShadowGrid.FadeOut();
                }
            }
        }
    }
}
