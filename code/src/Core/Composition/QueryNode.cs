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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Composition
{
    public class QueryNode
    {
        private const string ContextPrefix = "$";

        public QueryNode(string field, string @operator, string value)
        {
            IsContext = field.StartsWith(ContextPrefix);
            Field = field?.Replace(ContextPrefix, string.Empty);
            Operator = ParseOperator(@operator);
            Value = value;
        }

        private QueryOperator ParseOperator(string @operator)
        {
            if (@operator?.Equals("==") == true)
            {
                return QueryOperator.Equals;
            }
            else
            {
                return QueryOperator.NotEquals;
            }
        }

        public string Field { get; set; }
        public QueryOperator Operator { get; set; }
        public string Value { get; set; }

        public bool IsContext { get; }
    }

    public enum QueryOperator
    {
        Equals,
        NotEquals
    }
}
