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

namespace Localization
{
    internal class VisualBasicProjectTemplateGenerator : ProjectTemplateGeneratorBase
    {
        protected override string sourceDirRelPath => @"\code\src\ProjectTemplates\VBNet.UWP.VS2017.Solution";
        protected override string sourceDirNamePattern => "VBNet.UWP.VS2017.Solution";
        protected override string sourceFileNamePattern => "VBNet.UWP.VS2017.Solution.vstemplate";
        protected override string destinationDirNamePattern => "VBNet.UWP.VS2017.Solution";
        protected override string destinationFileNamePattern => "VBNet.UWP.VS2017.Solution.{0}.vstemplate";
        public VisualBasicProjectTemplateGenerator(string sourceDirPath, string destinationDirPath) : base(sourceDirPath, destinationDirPath)
        {
        }
    }
}
