// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace VsTemplates.Test.Models
{
    public class VerifierResultTestModel
    {
        public VerifierResultTestModel()
        {
        }

        public VerifierResultTestModel(bool success, string message)
            : this()
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
