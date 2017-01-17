using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Dialog
{
    public enum AddProjectSteps
    {
        SelectProject = 0,
        //SelectFeatures = 1,
        ShowSummary = 1 //Prev 2
    }

    public static class Extensions
    {
        public static AddProjectSteps Next(this AddProjectSteps step)
        {
            if (step != AddProjectSteps.ShowSummary)
            {
                int current = (int)step;
                current++;
                return (AddProjectSteps)current;
            }
            else
            {
                return step;
            }
        }
        public static AddProjectSteps Previous(this AddProjectSteps step)
        {
            int current = (int)step;
            if (current > 0)
            {
                current--;
                return (AddProjectSteps)current;
            }
            else
            {
                return step;
            }
        }
    }
}
