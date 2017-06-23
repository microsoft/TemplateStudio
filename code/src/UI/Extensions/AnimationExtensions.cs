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
                throw new ArgumentNullException("element");
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
                throw new ArgumentNullException("element");
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
                throw new ArgumentNullException("element");
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
            Storyboard storyboard = AnimateDoubleProperty(target, property, from, to, duration, easingFunction);
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
