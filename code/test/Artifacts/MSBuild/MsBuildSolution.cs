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
using System.Linq;

namespace Microsoft.Templates.Test.Artifacts.MSBuild
{
    public class MSBuildSolution
    {
        private const string GlobalSectionText = "GlobalSection(ProjectConfigurationPlatforms) = postSolution";

        private const string ConfigurationTemplate = @"		{0}.Debug|ARM.ActiveCfg = Debug|ARM
		{0}.Debug|ARM.Build.0 = Debug|ARM
		{0}.Debug|ARM.Deploy.0 = Debug|ARM
		{0}.Debug|x64.ActiveCfg = Debug|x64
		{0}.Debug|x64.Build.0 = Debug|x64
		{0}.Debug|x64.Deploy.0 = Debug|x64
		{0}.Debug|x86.ActiveCfg = Debug|x86
		{0}.Debug|x86.Build.0 = Debug|x86
		{0}.Debug|x86.Deploy.0 = Debug|x86
		{0}.Release|ARM.ActiveCfg = Release|ARM
		{0}.Release|ARM.Build.0 = Release|ARM
		{0}.Release|ARM.Deploy.0 = Release|ARM
		{0}.Release|x64.ActiveCfg = Release|x64
		{0}.Release|x64.Build.0 = Release|x64
		{0}.Release|x64.Deploy.0 = Release|x64
		{0}.Release|x86.ActiveCfg = Release|x86
		{0}.Release|x86.Build.0 = Release|x86
		{0}.Release|x86.Deploy.0 = Release|x86
";

        private const string ProjectTemplate = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""{name}"", ""{name}\{name}.csproj"", ""{id}""
EndProject
";

        private readonly string _path;

        private MSBuildSolution(string path)
        {
            _path = path;
        }

        public static MSBuildSolution Create(string path)
        {
            var solutionTemplate = ReadTemplate();
            File.WriteAllText(path, solutionTemplate);

            return new MSBuildSolution(path);
        }

        public void AddProjectToSolution(string projectName, string projectGuid)
        {
            var slnContent = File.ReadAllText(_path);

            if (slnContent.IndexOf(projectName) == -1)
            {
                var globalIndex = slnContent.IndexOf("Global");
                var projectContent = ProjectTemplate
                                            .Replace("{name}", projectName)
                                            .Replace("{id}", projectGuid);

                slnContent = slnContent.Insert(globalIndex, projectContent);

                var GlobalSectionIndex = slnContent.IndexOf(GlobalSectionText);
                var projectConfigContent = string.Format(ConfigurationTemplate, projectGuid);

                slnContent = slnContent.Insert(GlobalSectionIndex + GlobalSectionText.Length + 1, projectConfigContent);

            }

            File.WriteAllText(_path, slnContent);
        }

        private static string ReadTemplate()
        {
            return File.ReadAllText(@"MSBuild\SolutionTemplate.txt");
        }
    }
}
