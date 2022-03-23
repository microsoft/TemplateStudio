// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Templates.Core.Composition
{
    public class QueryNode
    {
        private const string ContextPrefix = "$";

        public string Field { get; set; }

        public QueryOperator Operator { get; set; }

        public string Value { get; set; }

        public bool IsContext { get; }

        public QueryNode(string field, string @operator, string value)
        {
            IsContext = field.StartsWith(ContextPrefix, StringComparison.Ordinal);
            Field = field?.Replace(ContextPrefix, string.Empty);
            Operator = ParseOperator(@operator);
            Value = value;
        }

        private QueryOperator ParseOperator(string @operator)
        {
            if (@operator?.Equals("==", StringComparison.Ordinal) == true)
            {
                return QueryOperator.Equals;
            }
            else
            {
                return QueryOperator.NotEquals;
            }
        }
    }
}
