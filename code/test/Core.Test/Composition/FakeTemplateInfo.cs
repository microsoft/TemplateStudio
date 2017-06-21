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
        public IReadOnlyDictionary<string, ICacheTag> Tags => _tags;
        public string GroupIdentity { get; set; }

        public void AddTag(string key, string value)
        {
            var cacheTag = new CacheTag("", new Dictionary<string, string>(), value);
            _tags.Add(key, cacheTag);
        }

        public string Author => String.Empty;

        public string Description => String.Empty;

        public IReadOnlyList<string> Classifications => null;

        public string DefaultName => String.Empty;

        public Guid GeneratorId => Guid.Empty;

        public string ShortName => String.Empty;

        public Guid ConfigMountPointId => Guid.Empty;

        public string ConfigPlace => String.Empty;

        public Guid LocaleConfigMountPointId => Guid.Empty;

        public string LocaleConfigPlace => String.Empty;

        public int Precedence => -1;
            
        public IReadOnlyDictionary<string, ICacheParameter> CacheParameters => null;

        public IReadOnlyList<ITemplateParameter> Parameters => null;

        public Guid HostConfigMountPointId => Guid.Empty;

        public string HostConfigPlace => String.Empty;

        public string ThirdPartyNotices => String.Empty;

        public IReadOnlyDictionary<string, IBaselineInfo> BaselineInfo => null;
    }
}
