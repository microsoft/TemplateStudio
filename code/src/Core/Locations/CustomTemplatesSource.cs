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

using System.IO;
using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Locations
{
    public class CustomTemplatesSource : TemplatesSource
    {
        public override string Id { get => "Custom"; }

        // TODO [ML]: add ability to set this
        // TODO [ML]: get this from settings
        public string Origin { get => $@"C:\MyLocalWtsTemplates\"; }

        public override void Adquire(string targetFolder)
        {
            if (Origin != null && Directory.Exists(Origin))
            {
                AppHealth.Current.Info.TrackAsync($"Loading local templates from: {Origin}").FireAndForget();

                // TODO [ML]: need to get version properly
                var targetVersionFolder = Path.Combine(targetFolder, "0.0.0.0");
                Fs.CopyRecursive(Origin, targetVersionFolder);
            }
            else
            {
                AppHealth.Current.Info.TrackAsync($"No local templates loaded").FireAndForget();
            }
        }
    }
}
