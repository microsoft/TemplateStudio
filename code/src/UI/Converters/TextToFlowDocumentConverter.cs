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

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using Microsoft.Templates.UI.Controls;

namespace Microsoft.Templates.UI.Converters
{
    /// <summary>
    /// This code has been taken from https://github.com/theunrepentantgeek/Markdown.XAML
    /// Authored by Bevan Arps under MIT license
    /// </summary>
    public class TextToFlowDocumentConverter : DependencyObject, IValueConverter
    {
        private Lazy<Markdown> _markdown = new Lazy<Markdown>(() => new Markdown());

        public Markdown Markdown
        {
            get => (Markdown)GetValue(MarkdownProperty);
            set => SetValue(MarkdownProperty, value);
        }

        // Using a DependencyProperty as the backing store for Markdown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkdownProperty =
            DependencyProperty.Register("Markdown", typeof(Markdown), typeof(TextToFlowDocumentConverter), new PropertyMetadata(null));

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var text = (string)value;
            var engine = Markdown ?? _markdown.Value;

            return engine.Transform(text);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
