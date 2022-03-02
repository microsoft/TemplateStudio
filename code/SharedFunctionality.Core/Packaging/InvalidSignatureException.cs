// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Core.Packaging
{
    [Serializable]
    public class InvalidSignatureException : Exception
    {
        public InvalidSignatureException()
        {
        }

        public InvalidSignatureException(string message)
            : base(message)
        {
        }

        public InvalidSignatureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidSignatureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
