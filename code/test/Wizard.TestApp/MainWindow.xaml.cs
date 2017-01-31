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

using Microsoft.Templates.Wizard.Host;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.Wizard.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TemplatesGen _gen;
        private FakeGenShell _shell;

        public MainWindow()
        {
            InitializeComponent();
            InitTester();
        }


        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _shell = new FakeGenShell(SeedSolName.Text + DateTime.Now.ToString("MMddmmss"), Status);
                _gen = new TemplatesGen(_shell, new TemplatesRepository(new LocalTemplatesLocation()));

                var genItems = _gen.GetUserSelection(WizardSteps.Project);
                if (genItems != null)
                {
                    _gen.Generate(genItems);

                    SolutionPath.Text = _shell.OutputPath;

                    LockProjectActions(); 
                }
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

                _shell = new FakeGenShell(projectName, Status);
                _gen = new TemplatesGen(_shell, new TemplatesRepository(new LocalTemplatesLocation()));

                var genItems = _gen.GetUserSelection(WizardSteps.Project);
                if (genItems != null)
                {
                    _gen.Generate(genItems);

                    SolutionPath.Text = _shell.OutputPath;

                    LockProjectActions();
                }
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
                _shell.UpdateRelativePath(PageRelativePath.Text);

                var genItems = _gen.GetUserSelection(WizardSteps.Page);
                if (genItems != null)
                {
                    _gen.Generate(genItems);
                }
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
            if (!String.IsNullOrEmpty(_shell.SolutionPath))
            {
                System.Diagnostics.Process.Start(_shell.SolutionPath);
            }
        }

        private void RestartTester_Click(object sender, RoutedEventArgs e)
        {
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
