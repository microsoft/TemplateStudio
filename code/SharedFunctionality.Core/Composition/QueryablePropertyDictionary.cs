// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Templates.Core.Composition
{
    [Serializable]
    public class QueryablePropertyDictionary : Dictionary<string, QueryableProperty>
    {
        protected QueryablePropertyDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public QueryablePropertyDictionary()
        {
        }

        public void Add(QueryableProperty property)
        {
            Add(property.Name, property);
        }

        public void AddOrUpdate(QueryableProperty property)
        {
            this[property.Name] = property;
        }

        public QueryableProperty SafeGet(string name)
        {
            return this.SafeGet(name, QueryableProperty.Empty);
        }
    }
}
