// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Fakes.GenShell
{
    public class FakeGenShellProject : IGenShellProject
    {
        public string Language { get; set; }

        public string Platform { get; set; }

        public string AppModel { get; set; }

        public FakeGenShellProject(string platform, string language, string appModel)
        {
            Platform = platform;
            Language = language;
            AppModel = appModel;
        }

        public bool GetActiveProjectIsWts()
        {
            return true;
        }

        public string GetActiveProjectLanguage()
        {
            return Language;
        }

        public string GetActiveProjectName()
        {
            return GenContext.Current.ProjectName;
        }

        public string GetActiveProjectNamespace()
        {
            return GenContext.Current.ProjectName;
        }

        public string GetActiveProjectPath()
        {
            return (GenContext.Current != null) ? GenContext.Current.DestinationPath : string.Empty;
        }

        public string GetActiveProjectTypeGuids()
        {
            var projectFileName = FindProject(GenContext.Current.DestinationPath);

            if (string.IsNullOrEmpty(projectFileName))
            {
                throw new Exception($"There is not project file in {GenContext.Current.DestinationPath}");
            }

            var msbuildProj = FakeMsBuildProject.Load(projectFileName);
            return msbuildProj.ProjectTypeGuids;
        }

        public Guid GetProjectGuidByName(string projectName)
        {
            var projectFileName = FindProject(GenContext.Current.DestinationPath);
            var msbuildProj = FakeMsBuildProject.Load(projectFileName);
            var guid = msbuildProj.Guid;
            if (string.IsNullOrEmpty(guid))
            {
                var solution = FakeSolution.LoadOrCreate(Platform, AppModel, Language, FakeGenShellHelper.SolutionPath);
                guid = solution.GetProjectGuids().First(p => p.Key == projectName).Value;
            }

            Guid.TryParse(guid, out Guid parsedGuid);
            return parsedGuid;
        }

        private static string FindProject(string path)
        {
            return Directory.EnumerateFiles(path, "*proj", SearchOption.AllDirectories).FirstOrDefault();
        }

        public bool IsActiveProjectWpf()
        {
            throw new NotImplementedException();
        }

        public bool IsActiveProjectWinUI()
        {
            throw new NotImplementedException();
        }

        public bool IsActiveProjectUwp()
        {
            throw new NotImplementedException();
        }
    }
}
