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
using System.Collections.Generic;
using System.IO;
using Microsoft.Templates.Core.Diagnostics;
using Newtonsoft.Json;

namespace Microsoft.Templates.Core.Locations
{
    public class CustomTemplatesSource : TemplatesSource
    {
        public override string Id { get => "Custom"; }

        //public string LocalPath { get => $@"C:\MyLocalWtsTemplates\"; }

        public string _localPath;

        public string LocalPath
        {
            get
            {
                if (_localPath == null)
                {
                    _localPath = CustomSettings.CustomTemplatePath;
                }

                return _localPath;
            }
        }

        public override void Adquire(string targetFolder)
        {
            if (LocalPath != null && Directory.Exists(LocalPath))
            {
                AppHealth.Current.Info.TrackAsync($"Loading local templates from: {LocalPath}").FireAndForget();

                // TODO [ML]: need to get version properly
                var targetVersionFolder = Path.Combine(targetFolder, "0.0.0.0");
                Fs.CopyRecursive(LocalPath, targetVersionFolder);
            }
            else
            {
                AppHealth.Current.Info.TrackAsync($"No local templates loaded").FireAndForget();
            }
        }
    }
}
