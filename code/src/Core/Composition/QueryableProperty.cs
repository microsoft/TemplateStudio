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

using System.Linq;

namespace Microsoft.Templates.Core.Composition
{
    public class QueryableProperty
    {
        public static QueryableProperty Empty => new QueryableProperty(string.Empty, string.Empty);

        public string Name { get; }
        public string Value { get; }

        public QueryableProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }


        public bool Compare(QueryNode query)
        {
            if (string.IsNullOrEmpty(Value) || string.IsNullOrEmpty(query.Value))
            {
                return false;
            }

            var comparitionResult = Compare(query.Value);

            if (query.Operator == QueryOperator.Equals)
            {
                return comparitionResult;
            }
            else
            {
                return !comparitionResult;
            }
        }

        private bool Compare(string toCompare)
        {
            if (Value.IsMultiValue() && toCompare.IsMultiValue())
            {
                return Value.GetMultiValue().Any(v => toCompare.GetMultiValue().Contains(v));
            }
            else if (Value.IsMultiValue() && !toCompare.IsMultiValue())
            {
                return Value.GetMultiValue().Any(v => v.Equals(toCompare));
            }
            else if (!Value.IsMultiValue() && toCompare.IsMultiValue())
            {
                return toCompare.GetMultiValue().Any(v => v.Equals(Value));
            }
            else
            {
                return Value.Equals(toCompare);
            }
        }
    }
}
