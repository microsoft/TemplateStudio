using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Microsoft.Templates.UI.V2Extensions
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
