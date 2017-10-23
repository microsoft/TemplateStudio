// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Core.Packaging
{
    [Serializable]
    public class SignCertNotFoundException : Exception
    {
        public SignCertNotFoundException()
        {
        }

        public SignCertNotFoundException(string message)
            : base(message)
        {
        }

        public SignCertNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SignCertNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
