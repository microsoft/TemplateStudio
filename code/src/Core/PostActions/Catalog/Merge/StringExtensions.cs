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
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class StringExtensions
    {
        public static int GetLeadingTrivia(this string statement)
        {
            return statement.TakeWhile(Char.IsWhiteSpace).Count();
        }

        public static string WithLeadingTrivia(this string statement, int triviaCount)
        {
            if (triviaCount < 1)
            {
                return statement;
            }
            else
            {
                return string.Concat(new string(' ', triviaCount), statement);
            }
        }
    }
}
