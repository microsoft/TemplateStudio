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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class FormattedWriterMessages
    {
        public const string ExHeader = "===================== Exception Info =====================";
        public const string ExFooter = "----------------------------------------------------------";
        public static string LogEntryStart
        {
            get
            {
                return $"[{DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff")}]\t{Environment.UserName}\t{Process.GetCurrentProcess().Id.ToString()}({System.Threading.Thread.CurrentThread.ManagedThreadId})\t";
            }
        }
    }
}
