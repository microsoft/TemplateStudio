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

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Utils;

namespace Microsoft.Templates.Core.Test.Composition
{
    public class FakeTemplateInfo : ITemplateInfo
    {
        private readonly Dictionary<string, ICacheTag> _tags = new Dictionary<string, ICacheTag>();

        public string Identity { get; set; }
        public string Name { get; set; }
        //public IReadOnlyDictionary<string, string> Tags => _tags;
        public IReadOnlyDictionary<string, ICacheTag> Tags => _tags;
        public string GroupIdentity { get; set; }

        public void AddTag(string key, string value)
        {
            var cacheTag = new CacheTag("", new Dictionary<string, string>(), value);
            _tags.Add(key, cacheTag);
        }

        public string Author => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public IReadOnlyList<string> Classifications => throw new NotImplementedException();

        public string DefaultName => throw new NotImplementedException();

        public Guid GeneratorId => throw new NotImplementedException();

        public string ShortName => throw new NotImplementedException();

        public Guid ConfigMountPointId => throw new NotImplementedException();

        public string ConfigPlace => throw new NotImplementedException();

        public Guid LocaleConfigMountPointId => throw new NotImplementedException();

        public string LocaleConfigPlace => throw new NotImplementedException();

        public int Precedence => throw new NotImplementedException();

        public IReadOnlyDictionary<string, ICacheParameter> CacheParameters => throw new NotImplementedException();

        public IReadOnlyList<ITemplateParameter> Parameters => throw new NotImplementedException();

        public Guid HostConfigMountPointId => throw new NotImplementedException();

        public string HostConfigPlace => throw new NotImplementedException();

        public string ThirdPartyNotices => throw new NotImplementedException();

        public IReadOnlyDictionary<string, IBaselineInfo> BaselineInfo => throw new NotImplementedException();
    }
}
