// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Core.Locations
{
    [Serializable]
    public class TemplateSynchronizationException : Exception
    {
        public TemplateSynchronizationException()
        {
        }

        public TemplateSynchronizationException(string message)
            : base(message)
        {
        }

        public TemplateSynchronizationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TemplateSynchronizationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
