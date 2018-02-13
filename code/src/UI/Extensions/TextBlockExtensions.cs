// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using Microsoft.Templates.UI.Controls;

namespace Microsoft.Templates.UI.Extensions
{
    public class TextBlockExtensions
    {
        public static readonly DependencyProperty FormatedTextProperty = DependencyProperty.RegisterAttached(
          "FormatedText",
          typeof(string),
          typeof(TextBlockExtensions),
          new PropertyMetadata(string.Empty, OnFormatedTextPropertyChanged));

        public static readonly DependencyProperty SequentialFlowStepProperty = DependencyProperty.RegisterAttached(
          "SequentialFlowStep",
          typeof(Step),
          typeof(TextBlockExtensions),
          new PropertyMetadata(null, OnSequentialFlowStepChanged));

        public static readonly DependencyProperty SequentialFlowStepCompletedProperty = DependencyProperty.RegisterAttached(
          "SequentialFlowStepCompleted",
          typeof(bool),
          typeof(TextBlockExtensions),
          new PropertyMetadata(false, OnSequentialFlowStepChanged));

        public static void SetFormatedText(UIElement element, string value)
        {
            element.SetValue(FormatedTextProperty, value);
        }

        public static string GetFormatedText(UIElement element)
        {
            return (string)element.GetValue(FormatedTextProperty);
        }

        public static void SetSequentialFlowStep(UIElement element, Step value)
        {
            element.SetValue(SequentialFlowStepProperty, value);
        }

        public static Step GetSequentialFlowStep(UIElement element)
        {
            return (Step)element.GetValue(SequentialFlowStepProperty);
        }

        public static void SetSequentialFlowStepCompleted(UIElement element, bool value)
        {
            element.SetValue(SequentialFlowStepCompletedProperty, value);
        }

        public static bool GetSequentialFlowStepCompleted(UIElement element)
        {
            return (bool)element.GetValue(SequentialFlowStepCompletedProperty);
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

        private static void OnSequentialFlowStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as TextBlock;
            var step = GetSequentialFlowStep(textBlock);
            var completed = GetSequentialFlowStepCompleted(textBlock);
            textBlock.Inlines.Clear();
            if (completed)
            {
                textBlock.Inlines.Add(new Run(step.LayoutIndex)
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Text = char.ConvertFromUtf32(0xE001).ToString()
                });
                textBlock.Inlines.Add($" {step.Title}");
            }
            else
            {
                textBlock.Inlines.Add($"{step.LayoutIndex} {step.Title}");
            }
        }
    }
}
