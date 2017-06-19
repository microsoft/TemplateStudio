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
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Gen
{
    public class GenContext
    {
        private static IContextProvider _currentContext;

        public static GenToolBox ToolBox { get; private set; }
        public static bool IsInitialized { get; private set; }

        public static IContextProvider Current
        {
            get
            {
                if (_currentContext == null)
                {
                    throw new InvalidOperationException("There is no context for the current gen execution, call Current_set first");
                }
                return _currentContext;
            }
            set
            {
                _currentContext = value;
            }
        }

        public static void Bootstrap(TemplatesSource source, GenShell shell)
        {
            Bootstrap(source, shell, GetWizardVersionFromAssembly());
        }

        public static void Bootstrap(TemplatesSource source, GenShell shell, Version wizardVersion)
        {
            AppHealth.Current.AddWriter(new ShellHealthWriter());
            AppHealth.Current.Info.TrackAsync($"Configuration file loaded: {Configuration.LoadedConfigFile}").FireAndForget();

            string hostVersion = $"{wizardVersion.Major}.{wizardVersion.Minor}";

            CodeGen.Initialize(source.Id, hostVersion);
            var repository = new TemplatesRepository(source, wizardVersion);

            ToolBox = new GenToolBox(repository, shell);

            IsInitialized = true;
        }

        private static Version GetWizardVersionFromAssembly()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);

            Version.TryParse(versionInfo.FileVersion, out Version v);

            return v;
        }
    }
}
