// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Microsoft.Templates.UI.Extensions
{
    public static class AnimationExtensions
    {
        public static Storyboard FadeIn(this UIElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (element.Opacity < 1.0)
            {
                return AnimateDoubleProperty(element, "Opacity", element.Opacity, 1.0, duration, easingFunction);
            }

            return null;
        }

        public static async Task FadeInAsync(this UIElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Opacity < 1.0)
            {
                await AnimateDoublePropertyAsync(element, "Opacity", element.Opacity, 1.0, duration, easingFunction);
            }
        }

        public static Storyboard FadeOut(this UIElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (element.Opacity > 0.0)
            {
                return AnimateDoubleProperty(element, "Opacity", element.Opacity, 0.0, duration, easingFunction);
            }

            return null;
        }

        public static async Task FadeOutAsync(this UIElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Opacity > 0.0)
            {
                await AnimateDoublePropertyAsync(element, "Opacity", element.Opacity, 0.0, duration, easingFunction);
            }
        }

        public static Storyboard AnimateWidth(this FrameworkElement element, double width, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (element.ActualWidth != width)
            {
                return AnimateDoubleProperty(element, "Width", element.ActualWidth, width, duration, easingFunction);
            }

            return null;
        }

        public static async Task AnimateWidthAsync(this FrameworkElement element, double width, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.ActualWidth != width)
            {
                await AnimateDoublePropertyAsync(element, "Width", element.ActualWidth, width, duration, easingFunction);
            }
        }

        public static Task AnimateDoublePropertyAsync(this DependencyObject target, string property, double from, double to, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            // TODO: Ensure this is properly callled in the async world. if so, remove the in-line suppresion and move it to the suppresion file.
#pragma warning disable VSTHRD103 // Llame a métodos asincrónicos cuando esté en un método asincrónico
            Storyboard storyboard = AnimateDoubleProperty(target, property, from, to, duration, easingFunction);
#pragma warning restore VSTHRD103 // Llame a métodos asincrónicos cuando esté en un método asincrónico
            storyboard.Completed += (sender, e) =>
            {
                tcs.SetResult(true);
            };
            return tcs.Task;
        }

        public static Storyboard AnimateDoubleProperty(this DependencyObject target, string property, double from, double to, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easingFunction ?? new SineEase(),
                FillBehavior = FillBehavior.HoldEnd
            };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property, null));

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Begin();

            return storyboard;
        }
    }
}
