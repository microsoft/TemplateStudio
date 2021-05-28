// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Windows;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Fakes.GenShell
{
    public class FakeGenShell : IGenShell
    {
        public string SolutionPath
        {
            get
            {
                if (GenContext.Current != null)
                {
                    return Path.Combine(Path.GetDirectoryName(GenContext.Current.DestinationPath), $"{GenContext.Current.ProjectName}.sln");
                }

                return null;
            }
        }

        public FakeGenShell(string platform, string language, Action<string> changeStatus = null, Action<string> addLog = null, Window owner = null)
        {
            Project = new FakeGenShellProject(platform, language, string.Empty, SolutionPath);
            Solution = new FakeGenShellSolution(platform, language, string.Empty, SolutionPath);
            Telemetry = new FakeGenShellTelemetry();
            UI = new FakeGenShellUI(changeStatus, addLog, owner);
            VisualStudio = new FakeGenShellVisualStudio();
            Certificate = new FakeGenShellCertificate();
        }

        public IGenShellProject Project { get; }

        public IGenShellSolution Solution { get; }

        public IGenShellTelemetry Telemetry { get; }

        public IGenShellUI UI { get; }

        public IGenShellVisualStudio VisualStudio { get; }

        public IGenShellCertificate Certificate { get; }

        public void SetCurrentLanguage(string language)
        {
            (Project as FakeGenShellProject).Language = language;
            (Solution as FakeGenShellSolution).Language = language;
        }

        public void SetCurrentPlatform(string platform)
        {
            (Project as FakeGenShellProject).Platform = platform;
            (Solution as FakeGenShellSolution).Platform = platform;
        }

        public void SetCurrentAppModel(string appModel)
        {
            (Project as FakeGenShellProject).AppModel = appModel;
            (Solution as FakeGenShellSolution).AppModel = appModel;
        }
    }
}
