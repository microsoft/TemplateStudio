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

using System.ComponentModel;
using System.Windows;

namespace Microsoft.Templates.UI.Services
{
    public class SystemService : DependencyObject
    {
        private static SystemService _instance;
        public static SystemService Instance => _instance ?? (_instance = new SystemService());

        private SystemService()
        {
            SystemParameters.StaticPropertyChanged += OnStaticPropertyChanged;
        }

        private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HighContrast")
            {
                Instance.IsHighContrast = SystemParameters.HighContrast;
            }
        }

        public static readonly DependencyProperty IsHighContrastProperty = DependencyProperty.Register("IsHighContrast", typeof(bool), typeof(SystemService), new PropertyMetadata(SystemParameters.HighContrast));
        public bool IsHighContrast
        {
            get => (bool)GetValue(IsHighContrastProperty);
            private set => SetValue(IsHighContrastProperty, value);
        }
    }
}
