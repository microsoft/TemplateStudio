using Microsoft.Templates.Wizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.Wizard.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TestVsShell testShell;
        GenerationWizard genWizard;

        public MainWindow()
        {
            InitializeComponent();
            InitTester();
        }


        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                testShell = new TestVsShell(SeedSolName.Text + DateTime.Now.ToString("MMddmmss"), DefaultNamespace.Text, Status);
                genWizard = new GenerationWizard(testShell, testShell.VsData.Solution,new TemplatesRepository(new TestTemplatesLocation()) );

                //TODO: MAYBE IS CANCELLED
                genWizard.AddProjectInit();
                genWizard.AddProjectFinish();

                SolutionPath.Text = testShell.VsData.Solution.FullName;

                LockProjectActions();
            }
            catch (WizardCancelledException)
            {
                MessageBox.Show($"User Cancelled!", "Wizard exited", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Exception: {ex.ToString()}", "Wizard exited", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetActiveProject_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(PathToExistingProject.Text) && File.Exists(PathToExistingProject.Text) && PathToExistingProject.Text.EndsWith("proj"))
            {
                string projectName = System.IO.Path.GetFileNameWithoutExtension(PathToExistingProject.Text);
                testShell = new TestVsShell(projectName, projectName, Status);
                genWizard = new GenerationWizard(testShell, testShell.VsData.Solution);
                testShell.AddProjectToSolution(PathToExistingProject.Text);
                SolutionPath.Text = PathToExistingProject.Text;
                LockProjectActions();
            }
            else
            {
                MessageBox.Show("Path is empty, the project does not exists or is not a valid .*proj file", "Set existing project", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void AddPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                testShell.UpdateSelectedItemPath(PageRelativePath.Text);
                genWizard.AddPageToActiveProject();
            }
            catch (WizardCancelledException)
            {
                MessageBox.Show($"User Cancelled!", "Wizard exited", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Exception: {ex.ToString()}", "Wizard exited", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddFeature_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                genWizard.AddFeatureToActiveProject();
            }
            catch (WizardCancelledException)
            {
                MessageBox.Show($"User Cancelled!", "Wizard exited", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Exception: {ex.ToString()}", "Wizard exited", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void OpenInVs_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(testShell.VsData.Solution.FullName))
            {
                System.Diagnostics.Process.Start(testShell.VsData.Solution.FullName);
            }
        }

        private void RestartTester_Click(object sender, RoutedEventArgs e)
        {
            testShell = null;
            InitTester();
        }
        private void InitTester()
        {
            SolutionPath.Text = "";
            PagePath.Text = "";
            AddProject.IsEnabled = true;
            SetActiveProject.IsEnabled = true;
            AddPage.IsEnabled = false;
            AddFeature.IsEnabled = false;
            OpenInVs.Visibility = Visibility.Hidden;
            RestartTester.Visibility = Visibility.Hidden;
            Status.Text = "";
            PageRelativePath.Text = "";
        }

        private void LockProjectActions()
        {
            AddProject.IsEnabled = false;
            SetActiveProject.IsEnabled = false;
            AddPage.IsEnabled = true;
            AddFeature.IsEnabled = true;
            OpenInVs.Visibility = Visibility.Visible;
            RestartTester.Visibility = Visibility.Visible;
        }
    }
}
