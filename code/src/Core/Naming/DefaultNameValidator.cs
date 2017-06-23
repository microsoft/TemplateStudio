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

using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core
{
    public class DefaultNamesValidator : Validator
    {
        private static readonly Lazy<string[]> _defaultNames = new Lazy<string[]>(() => GetDefaultNames());
        public static string[] DefaultNames => _defaultNames.Value;

        private static string[] GetDefaultNames()
        {
            return GenContext.ToolBox.Repo.Get(t => !t.GetItemNameEditable()).Select(n => n.GetDefaultName()).ToArray();
        }

        public override ValidationResult Validate(string suggestedName)
        {
            if (DefaultNames.Contains(suggestedName))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.ReservedName
                };
            }
            else
            {
                return new ValidationResult()
                {
                    IsValid = true,
                    ErrorType = ValidationErrorType.None
                };
            }
        }
    }
}
