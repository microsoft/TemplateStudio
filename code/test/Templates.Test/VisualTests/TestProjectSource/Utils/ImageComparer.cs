// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace AutomatedUITests.Utils
{
    public static class ImageComparer
    {
        // Factor for smaller image conversion
        public static int DivFactor = 10;

        private static readonly Pen DiffImageOutlinePen = Pens.DeepPink;

        private static readonly int DiffImageGridSize = 16;

        private static readonly ColorMatrix GrayScaleColorMatrix = new ColorMatrix(new[]
        {
            new[] {.3f, .3f, .3f, 0, 0},
            new[] {.59f, .59f, .59f, 0, 0},
            new[] {.11f, .11f, .11f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
        });

        public static float PercentageDifferent(Image img1, Image img2)
        {
            var differences = img1.GetDifferences(img2);
            var diffPixels = differences.Cast<byte>().Count(b => b > 1);

            return diffPixels / 256f;
        }

        public static Image GetDifferenceImage(this Image img1, Image img2)
        {
            int width = img1.Width / DivFactor, height = img1.Height / DivFactor;
            var differences = img1.GetDifferences(img2);
            var originalImage = new Bitmap(img1, width * DiffImageGridSize + 1, height * DiffImageGridSize + 1);
            var g = Graphics.FromImage(originalImage);

            for (var y = 0; y < differences.GetLength(1); y++)
            {
                for (var x = 0; x < differences.GetLength(0); x++)
                {
                    if (differences[x, y] > 1)
                    {
                        g.DrawRectangle(DiffImageOutlinePen, x * DiffImageGridSize, y * DiffImageGridSize, DiffImageGridSize, DiffImageGridSize);
                    }
                }
            }

            return originalImage.Resize(img1.Width, img1.Height);
        }

        public static byte[,] GetDifferences(this Image img1, Image img2)
        {
            int width = img1.Width / DivFactor, height = img1.Height / DivFactor;
            var thisOne = (Bitmap)img1.Resize(width, height).GetGrayScaleVersion();
            var theOtherOne = (Bitmap)img2.Resize(width, height).GetGrayScaleVersion();
            var differences = new byte[width, height];

            // exclusion area to cover app name (which may be different)
            int exclusionAreaWidth = 600 / DivFactor;
            int exclusionAreaHeight = 40 / DivFactor;

            for (var h = 0; h < height; h++)
            {
                for (var w = 0; w < width; w++)
                {
                    if (w <= exclusionAreaWidth && h <= exclusionAreaHeight)
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
    }
}
