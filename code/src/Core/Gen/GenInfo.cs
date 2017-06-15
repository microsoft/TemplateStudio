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

using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Core.Gen
{
    public class GenInfo
    {
        public string Name { get; set; }
        public ITemplateInfo Template { get; set; }
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public string GetUserName()
        {
            if (Parameters.ContainsKey(GenParams.Username))
            {
                return Parameters[GenParams.Username];
            }
            return string.Empty;
        }

        public override string ToString()
        {
            return $"{Name}, {Template?.Name}";
        }
    }
}
