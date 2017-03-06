using System.IO;
using System.Linq;

namespace Microsoft.Templates.Test.Artifacts
{
    public class SolutionFile
    {

        private string _path;

        private SolutionFile(string path)
        {
            //TODO: CHECK FILE EXISTS
            _path = path;
        }

        public static SolutionFile Load(string path)
        {
            return new SolutionFile(path);
        }

        public void AddProjectToSolution(string projectName, string projectGuid)
        {
            var slnContent = File.ReadAllLines(_path).ToList();

            if (!slnContent.Any(d => d.Contains(projectName)))
            {
                var index = slnContent.LastIndexOf("EndProject");

                string projectDef = "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"{ProjName}\", \"{ProjName}\\{ProjName}.csproj\", \"{ProjGuid}\"";
                projectDef = projectDef.Replace("{ProjName}", projectName);
                projectDef = projectDef.Replace("{ProjGuid}", projectGuid);

                slnContent.Insert(index, projectDef);
                slnContent.Insert(index, "EndProject");
            }

            File.WriteAllLines(_path, slnContent);
        }
    }
}
