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
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Extension
{
    internal static class ResourcesExtensions
    {
        public static string UseParams(this string res, params object[] args)
        {
            string pattern = @"(\{\d\})+";

            if (Regex.IsMatch(res, pattern))
            {
                try
                {
                    return string.Format(res, args);
                }
                catch
                {
                    return res + "<INVALID FORMAT>";
                }
            }
            else
            {
                return res;
            }
        }
    }
}
