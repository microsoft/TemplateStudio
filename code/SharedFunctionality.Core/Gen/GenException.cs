// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.Gen
{
    [Serializable]
    public class GenException : Exception
    {
        protected GenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GenException(string message)
            : base(message)
        {
        }

        public GenException(string name, string template, string reason)
            : base(string.Format(Resources.ErrorGenerating, template, name, reason))
        {
        }
    }
}
