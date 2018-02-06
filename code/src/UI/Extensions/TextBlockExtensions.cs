// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Microsoft.Templates.UI.Extensions
{
    public class TextBlockExtensions
    {
        public static readonly DependencyProperty FormatedTextProperty = DependencyProperty.RegisterAttached(
          "FormatedText",
          typeof(string),
          typeof(TextBlockExtensions),
          new PropertyMetadata(string.Empty, OnFormatedTextPropertyChanged));

        public static void SetFormatedText(UIElement element, string value)
        {
            element.SetValue(FormatedTextProperty, value);
        }

        public static string GetFormatedText(UIElement element)
        {
            return (string)element.GetValue(FormatedTextProperty);
        }

        private static void OnFormatedTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as TextBlock;
            var formatedText = GetFormatedText(textBlock);
            var formatSplits = formatedText.Split('*');
            bool isBold = false;
            textBlock.Inlines.Clear();
            foreach (var split in formatSplits)
            {
                if (isBold)
                {
                    textBlock.Inlines.Add(new Run(split) { FontWeight = FontWeights.Bold });
                }
                else
                {
                    textBlock.Inlines.Add(split);
                }

                isBold = !isBold;
            }
        }
    }
}
