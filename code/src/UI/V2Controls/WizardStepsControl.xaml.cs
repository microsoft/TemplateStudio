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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Templates.UI.V2Controls
{
    /// <summary>
    /// Interaction logic for WizardStepsControl.xaml
    /// </summary>
    public partial class WizardStepsControl : UserControl
    {

        public IEnumerable<WizardStep> Steps
        {
            get { return (IEnumerable<WizardStep>)GetValue(StepsProperty); }
            set { SetValue(StepsProperty, value); }
        }

        public static readonly DependencyProperty StepsProperty = DependencyProperty.Register("Steps", typeof(IEnumerable<WizardStep>), typeof(WizardStepsControl), new PropertyMetadata(GetSteps()));

        private static IEnumerable<WizardStep> GetSteps()
        {
            return new List<WizardStep>()
            {
                new WizardStep(1, true, "Project configuration"),
                new WizardStep(2,  false, "Add pages"),
                new WizardStep(3, false, "Add features")
            };
        }

        public WizardStepsControl()
        {
            InitializeComponent();
        }
    }
}
