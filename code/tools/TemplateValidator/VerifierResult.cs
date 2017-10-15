// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace TemplateValidator
{
    public class VerifierResult
    {
        public VerifierResult()
        {
        }

        public VerifierResult(bool success, List<string> messages)
            : this()
        {
            Success = success;
            Messages = messages;
        }

        public bool Success { get; set; }

        public List<string> Messages { get; set; }
    }
}
