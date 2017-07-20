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
    internal class CSharpProjectTemplateGenerator : ProjectTemplateGeneratorBase
    {
        protected override string sourceDirRelPath => @"\code\src\ProjectTemplates\CSharp.UWP.2017.Solution";
        protected override string sourceDirNamePattern => "CSharp.UWP.2017.Solution";
        protected override string sourceFileNamePattern => "CSharp.UWP.VS2017.Solution.vstemplate";
        protected override string destinationDirNamePattern => "CSharp.UWP.2017.Solution";
        protected override string destinationFileNamePattern => "CSharp.UWP.VS2017.Solution.{0}.vstemplate";
        public CSharpProjectTemplateGenerator(string sourceDirPath, string destinationDirPath) : base(sourceDirPath, destinationDirPath)
        {
        }
    }
}
