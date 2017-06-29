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
using System.IO;
using System.Linq;

namespace Microsoft.Templates.Core
{
    public class FileExistsValidator : Validator<string>
    {
        public FileExistsValidator(string config) : base(config)
        {
        }

        public override ValidationResult Validate(string suggestedName)
        {
            var existing = Directory.EnumerateFiles(_config)
                                            .Select(f => Path.GetFileNameWithoutExtension(f))
                                            .ToList();

            if (existing.Contains(suggestedName))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.AlreadyExists
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
