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
using System.Runtime.Serialization;

using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Core
{
    [Serializable]
    public class RepositorySynchronizationException : Exception
    {
        public RepositorySynchronizationException()
        {
        }

        public RepositorySynchronizationException(string message, Exception innerException = null) : base(message, innerException)
        {
        }

        protected RepositorySynchronizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RepositorySynchronizationException(SyncStatus status, Exception innerException = null) : base($"Error syncing templates. Status: '{status}'", innerException)
        {
        }
    }
}