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
        public static Task AnimateDoublePropertyAsync(this DependencyObject target, string property, double from, double to, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            // TODO: Ensure this is properly callled in the async world. if so, remove the in-line suppresion and move it to the suppresion file.
            Storyboard storyboard = AnimateDoubleProperty(target, property, from, to, duration, easingFunction);
            void Callback(object sender, EventArgs e)
            {
                storyboard.Completed -= Callback;
                tcs.SetResult(true);
            }

            storyboard.Completed += Callback;
            storyboard.Begin();
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
                FillBehavior = FillBehavior.HoldEnd,
            };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property, null));

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;

            return storyboard;
        }
    }
}
