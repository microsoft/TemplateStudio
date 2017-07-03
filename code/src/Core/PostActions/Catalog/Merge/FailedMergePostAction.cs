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

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class FailedMergePostAction
    {
        public MergeFailureType MergeFailureType { get; private set; }

        public string FileName { get; private set; }

        public string FailedFileName { get; private set; }

        public string FilePath { get; set; }

        public string Description { get; private set; }

        public FailedMergePostAction(string fileName, string filePath, string failedFileName, string description, MergeFailureType mergeFailureType)
        {
            FileName = fileName;
            FilePath = filePath;
            FailedFileName = failedFileName;
            Description = description;
            MergeFailureType = mergeFailureType;
        }
    }
}
