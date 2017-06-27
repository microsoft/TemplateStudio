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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Resources;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class RightClickActions
    {
        public static void AddNewPage()
        {
            try
            {
                var userSelection = NewItemGenController.Instance.GetUserSelectionNewPage();

                if (userSelection != null)
                {
                    NewItemGenController.Instance.FinishGeneration(userSelection);
                    GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.NewItemAddPageSuccessStatusMsg, userSelection.Pages[0].name));
                }
            }
            catch (WizardBackoutException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage(StringRes.NewItemAddPageCancelled);
            }
        }

        public static void AddNewFeature()
        {
            try
            {
                var userSelection = NewItemGenController.Instance.GetUserSelectionNewFeature();

                if (userSelection != null)
                {
                    NewItemGenController.Instance.FinishGeneration(userSelection);
                    GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Format(StringRes.NewItemAddFeatureSuccessStatusMsg, userSelection.Features[0].name));
                }
            }
            catch (WizardBackoutException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage(StringRes.NewItemAddFeatureCancelled);
            }
        }

        public static bool Enabled()
        {
            // TODO: Depends if the current project has been created with WTS or not
            return true;
        }
    }
}
