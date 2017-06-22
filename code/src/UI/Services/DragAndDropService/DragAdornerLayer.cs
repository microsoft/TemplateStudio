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
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Templates.UI.Services
{
    public class DragAdornerLayer : Adorner
    {
        private Rectangle _child = null;
        private double _offsetLeft = 0;
        private double _offsetTop = 0;

        public DragAdornerLayer(UIElement adornedElement, Size size, Brush brush) : base(adornedElement)
        {
            Rectangle rect = new Rectangle()
            {
                Fill = brush,
                Width = size.Width,
                Height = size.Height,
                IsHitTestVisible = false
            };

            _child = rect;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();

            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_offsetLeft, _offsetTop));

            return result;
        }

        public double OffsetLeft
        {
            get => _offsetLeft;
            set
            {
                _offsetLeft = value;

                UpdateLocation();
            }
        }

        public void SetOffsets(double left, double top)
        {
            _offsetLeft = left;
            _offsetTop = top;

            UpdateLocation();
        }

        public double OffsetTop
        {
            get => _offsetTop;
            set
            {
                _offsetTop = value;

                UpdateLocation();
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _child.Measure(constraint);

            return _child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _child.Arrange(new Rect(finalSize));

            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => _child;

        protected override int VisualChildrenCount => 1;

        private void UpdateLocation()
        {
            if (Parent is AdornerLayer adornerLayer)
            {
                adornerLayer.Update(AdornedElement);
            }
        }
    }
}
