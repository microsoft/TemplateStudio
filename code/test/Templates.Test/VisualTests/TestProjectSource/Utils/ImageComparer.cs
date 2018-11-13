// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace AutomatedUITests.Utils
{
    public static class ImageComparer
    {
        // Factor for smaller image conversion
        public static int DivFactor = 10;

        private static readonly Pen DiffImageOutlinePen = Pens.DeepPink;
        private static readonly Pen DiffExclusionOutlinePen = Pens.Green;

        private static readonly int DiffImageGridSize = 16;

        private static readonly ColorMatrix GrayScaleColorMatrix = new ColorMatrix(new[]
        {
            new[] {.3f, .3f, .3f, 0, 0},
            new[] {.59f, .59f, .59f, 0, 0},
            new[] {.11f, .11f, .11f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
        });

        public static float PercentageDifferent(Image img1, Image img2, params ExclusionArea[] exclusionAreas)
        {
            var differences = img1.GetDifferences(img2, exclusionAreas);
            var diffPixels = differences.Cast<byte>().Count(b => b > 1);

            return diffPixels / 256f;
        }

        public static Image GetDifferenceImage(this Image img1, Image img2, params ExclusionArea[] exclusionAreas)
        {
            var differences = img1.GetDifferences(img2, exclusionAreas);
            var originalImage = new Bitmap(img1);
            var g = Graphics.FromImage(originalImage);

            // Highlight areas where a difference was detected to make them obvious
            for (var y = 0; y < differences.GetLength(1); y++)
            {
                for (var x = 0; x < differences.GetLength(0); x++)
                {
                    if (differences[x, y] > 1)
                    {
                        g.DrawRectangle(DiffImageOutlinePen, x * DivFactor, y * DivFactor, DiffImageGridSize, DiffImageGridSize);
                    }
                }
            }

            // Highlight the excluded areas so it's clear why any differences there aren't shown
            foreach (var excludedArea in exclusionAreas)
            {
                var rect = AdjustExclusionAreaForThisDevice(excludedArea);

                g.DrawRectangle(DiffExclusionOutlinePen, rect.Left, rect.Top, rect.Width, rect.Height);
            }

            return originalImage.Resize(img1.Width, img1.Height);
        }

        public static byte[,] GetDifferences(this Image img1, Image img2, params ExclusionArea[] exclusionAreas)
        {
            int width = img1.Width / DivFactor, height = img1.Height / DivFactor;
            var thisOne = (Bitmap)img1.Resize(width, height).GetGrayScaleVersion();
            var theOtherOne = (Bitmap)img2.Resize(width, height).GetGrayScaleVersion();
            var differences = new byte[width, height];

            for (var h = 0; h < height; h++)
            {
                for (var w = 0; w < width; w++)
                {
                    var inExclusionArea = false;
                    foreach (var exclusionArea in exclusionAreas)
                    {
                        var rect = AdjustExclusionAreaForThisDevice(exclusionArea);

                        if (w >= rect.Left / DivFactor && w <= rect.Right / DivFactor
                         && h >= rect.Top / DivFactor && h <= rect.Bottom / DivFactor)
                        {
                            inExclusionArea = true;
                            break;
                        }
                    }

                    if (inExclusionArea)
                    {
                        differences[w, h] = 0;
                    }
                    else
                    {
                        // Just comparing Red color difference for speed and on the assumption image is greyscale
                        differences[w, h] = (byte)Math.Abs(thisOne.GetPixel(w, h).R - theOtherOne.GetPixel(w, h).R);
                    }
                }
            }

            return differences;
        }

        private static Rectangle AdjustExclusionAreaForThisDevice(ExclusionArea exclusionArea)
        {
            var thisDeviceScale = GetScreenScalingFactor();

            return new Rectangle(
                (int)(exclusionArea.Area.X / exclusionArea.ScaleFactor * thisDeviceScale),
                (int)(exclusionArea.Area.Y / exclusionArea.ScaleFactor * thisDeviceScale),
                (int)(exclusionArea.Area.Width / exclusionArea.ScaleFactor * thisDeviceScale),
                (int)(exclusionArea.Area.Height / exclusionArea.ScaleFactor * thisDeviceScale));
        }

        public static Image GetGrayScaleVersion(this Image original)
        {
            var newBitmap = new Bitmap(original.Width, original.Height);

            using (var g = Graphics.FromImage(newBitmap))
            {
                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(GrayScaleColorMatrix);

                g.DrawImage(
                    original,
                    new Rectangle(0, 0, original.Width, original.Height),
                    0,
                    0,
                    original.Width,
                    original.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }

            return newBitmap;
        }

        public static Image Resize(this Image originalImage, int newWidth, int newHeight)
        {
            var smallVersion = new Bitmap(newWidth, newHeight);

            using (var g = Graphics.FromImage(smallVersion))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return smallVersion;
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
        }

        public class ExclusionArea
        {
            public ExclusionArea()
            {
            }

            public ExclusionArea(Rectangle area, float scaleFactor)
            {
                Area = area;
                ScaleFactor = scaleFactor;
            }

            public Rectangle Area { get; set; }

            public float ScaleFactor { get; set; }
        }

        private static float scaleFactor = float.MinValue;

        // Returns 1.0 for 100%, 1.5 for 150% etc.
        public static float GetScreenScalingFactor()
        {
            // Cache this as won't change while tests are running and requesting frequently can error
            if (scaleFactor == float.MinValue)
            {
                Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                IntPtr desktop = g.GetHdc();
                int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
                int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

                float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

                scaleFactor = ScreenScalingFactor;
            }

            return scaleFactor;
        }
    }
}
