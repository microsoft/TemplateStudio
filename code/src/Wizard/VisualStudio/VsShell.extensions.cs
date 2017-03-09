using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.VisualStudio
{

    public static class VsShellExtensions
    {

        public static string GetSafeValue(this EnvDTE.Properties props, string propertyName)
        {
            if (props != null)
            {
                if(props.Cast<Property>().Where(p => p.Name == propertyName).Any())
                {
                    return props.Item(propertyName).Value.ToString();
                }
                else
                {
                    return $"NOT_FOUND_PROPERTY";
                }
            }
            else
            {
                return String.Empty;
            }
        }

        public static T GetSafeValue<T>(this EnvDTE.Properties props, string propertyName)
        {
            if (props != null)
            {
                if (props.Cast<Property>().Where(p => p.Name == propertyName).Any())
                {
                    return (T)props.Item(propertyName).Value;
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        public static string RemoveTailDirectorySparator(this string target)
        {
            if (target.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                return target.Substring(0, target.Length - 1);
            }
            else
            {
                return target;
            }
        }

        public static string RemoveStartDirectorySparator(this string target)
        {
            if (target.StartsWith(Path.DirectorySeparatorChar.ToString()))
            {
                return target.Substring(1, target.Length-1);
            }
            else
            {
                return target;
            }
        }

        //public static bool InstallNuGetPackage(this Project project, string packageName)
        //{
        //    bool installedPkg = true;

        //    try
        //    {
        //        var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));

        //        IVsPackageInstallerServices installerServices = componentModel.GetService<IVsPackageInstallerServices>();
        //        if (!installerServices.IsPackageInstalled(project, packageName))
        //        {
        //            $"Installing {packageName} NuGet package, this may take a minute...".VsShowStatusBarMessage();
        //            IVsPackageInstaller installer = componentModel.GetService<IVsPackageInstaller>();
        //            installer.InstallPackage(null, project, packageName, (System.Version)null, false);
        //            $"Finished installing the {packageName} NuGet package".VsShowStatusBarMessage();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        installedPkg = false;
        //        $"Unable to install the {packageName} NuGet package".VsShowStatusBarMessage();
        //        Debug.WriteLine($"Exception installing NuGet package: {ex.ToString()}");
        //    }

        //    return installedPkg;
        //}

        #region TO BE DELETED METHODS
        //public static SolutionFolder GetSolutionFolderEx(this Solution solution, string folderName)
        //{
        //    Project solutionFolder = (from p in ((Solution2)solution).Projects.OfType<Project>()
        //                              where p.Name.Equals(folderName)
        //                              select p).FirstOrDefault();

        //    return solutionFolder?.Object;
        //}

        //public static SolutionFolder GetSolutionFolderEx(this SolutionFolder solutionFolder, string folderName)
        //{
        //    ProjectItem folder = (from p in solutionFolder.Parent.ProjectItems.OfType<ProjectItem>()
        //                          where p.Name.Equals(folderName)
        //                          select p).FirstOrDefault();

        //    return ((Project)folder?.Object)?.Object;
        //}

        //public static SolutionFolder AddSolutionFolderEx(this Solution solution, string folderName)
        //{
        //    SolutionFolder folder = solution.GetSolutionFolderEx(folderName);

        //    if (folder == null)
        //    {
        //        folder = ((Solution4)solution).AddSolutionFolder(folderName).Object;
        //    }

        //    return folder;
        //}

        //public static SolutionFolder AddSolutionFolderEx(this SolutionFolder solutionFolder, string folderName)
        //{
        //    SolutionFolder folder = solutionFolder.GetSolutionFolderEx(folderName);

        //    if (folder == null)
        //    {
        //        folder = solutionFolder.AddSolutionFolder(folderName).Object;
        //    }

        //    return folder;
        //}

        //public static Project AddProject(this Solution solution, string destination, string projectName, string templateName)
        //{
        //    string projectPath = Path.Combine(destination, projectName);
        //    string templatePath = ((Solution4)solution).GetProjectTemplate(templateName, "CSharp");

        //    solution.AddFromTemplate(templatePath, projectPath, projectName, false);

        //    return solution.GetProject(projectName);
        //}

        //public static Project AddProject(this SolutionFolder solutionFolder, string destination, string projectName, string templateName)
        //{
        //    string projectPath = Path.Combine(destination, projectName);
        //    string templatePath = ((Solution4)solutionFolder.DTE.Solution).GetProjectTemplate(templateName, "CSharp");

        //    solutionFolder.AddFromTemplate(templatePath, projectPath, projectName);

        //    return solutionFolder.DTE.Solution.GetProject(projectName);
        //}

        //public static void AddItem(this Project project, string itemTemplateName, string itemName)
        //{
        //    string templatePath = ((Solution4)project.DTE.Solution).GetProjectItemTemplate(itemTemplateName, "CSharp");
        //    project.ProjectItems.AddFromTemplate(templatePath, itemName);
        //}


        //public static void SetAsStartup(this Project project)
        //{
        //    $"Set {project.Name} as the startup project".VsShowStatusBarMessage();
        //    DTE _dte = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE;
        //    _dte.Solution.Properties.Item("StartupProject").Value = project.Name;
        //}

        //public static void AddReference(this Project project, Project projectToAdd)
        //{
        //    $"Adding {projectToAdd.Name} as a reference in {project.Name}".VsShowStatusBarMessage();
        //    (project.Object as VSProject).References.AddProject(projectToAdd);
        //}

        //public static void SetResponsibility<T>(this Project project, params T[] responsibilities)
        //{
        //    foreach (var res in Enum.GetValues(typeof(T)))
        //    {
        //        string name = res.ToString();
        //        project.Globals[name] = Boolean.FalseString;
        //        project.Globals.set_VariablePersists(name, true);
        //    }

        //    foreach (var res in responsibilities)
        //    {
        //        string name = res.ToString();
        //        project.Globals[name] = Boolean.TrueString;
        //        project.Globals.set_VariablePersists(name, true);
        //    }
        //}

        //public static bool IsProjectResponsible(this Project project, Enum responsibility)
        //{
        //    if (project == null)
        //        throw new ArgumentNullException(nameof(project));

        //    if (project.Globals.get_VariableExists(responsibility.ToString()))
        //    {
        //        string propertyValue = (string)project.Globals[responsibility.ToString()];
        //        bool propertyValueBoolean;

        //        if (Boolean.TryParse(propertyValue, out propertyValueBoolean))
        //        {
        //            if (propertyValueBoolean)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        //public static Project GetProject(this Solution solution, string projectName) =>
        //    solution.GetAllProjects().Where(p => p.Name.Equals(projectName)).FirstOrDefault();

        //private static IEnumerable<Project> GetAllProjects(this Solution solution)
        //{
        //    return solution.Projects
        //          .Cast<Project>()
        //          .SelectMany(GetChildProjects)
        //          .Union(solution.Projects.Cast<Project>())
        //          .Where(p => { try { return !string.IsNullOrEmpty(p.FullName); } catch { return false; } });
        //}

        //private static IEnumerable<Project> GetChildProjects(Project parent)
        //{
        //    try
        //    {
        //        if (parent.Kind != ProjectKinds.vsProjectKindSolutionFolder && parent.Collection == null)  // Unloaded
        //            return Enumerable.Empty<Project>();

        //        if (!string.IsNullOrEmpty(parent.FullName))
        //            return new[] { parent };
        //    }
        //    catch (COMException)
        //    {
        //        return Enumerable.Empty<Project>();
        //    }

        //    return parent.ProjectItems
        //            .Cast<ProjectItem>()
        //            .Where(p => p.SubProject != null)
        //            .SelectMany(p => GetChildProjects(p.SubProject));
        //}
        #endregion
    }
}
