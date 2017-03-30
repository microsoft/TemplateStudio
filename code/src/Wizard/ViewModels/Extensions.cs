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
using System.IO;
using System.Windows.Media.Imaging;

namespace Microsoft.Templates.Wizard.ViewModels
{
    public static class Extensions
    {
        public const string DefaultProjectIcon = "pack://application:,,,/Microsoft.Templates.Wizard;component/Assets/DefaultProjectIcon.png";
        public static BitmapImage CreateIcon(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    return CreateBitMap(new Uri(DefaultProjectIcon));
                }
                else
                {
                    return CreateBitMap(new Uri(path));
                }
            }
            catch (IOException)
            {
                //SYNC AT SAME TIME IS LOADING THE ICON OR ICON IS LOCKED
                return CreateBitMap(new Uri(DefaultProjectIcon));
            }
        }

        private static BitmapImage CreateBitMap(Uri source)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = source;
            image.EndInit();
            return image;
        }
    }
}
