// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.Constraints;
using Microsoft.TemplateEngine.Abstractions.Parameters;

namespace Microsoft.Templates.Core.Test.Composition
{
    public class FakeTemplateInfo : ITemplateInfo
    {
        private readonly Dictionary<string, string> _tags = new Dictionary<string, string>();

        public string Identity { get; set; }

        public string Name { get; set; }

        [Obsolete]
        public IReadOnlyDictionary<string, ICacheTag> Tags { get; }

        public string GroupIdentity { get; set; }

        public void AddTag(string key, string value)
        {
            _tags.Add(key, value);
        }

        public string Author => string.Empty;

        public string Description => string.Empty;

        public IReadOnlyList<string> Classifications => null;

        public string DefaultName => string.Empty;

        public Guid GeneratorId => Guid.Empty;

        public string ShortName => string.Empty;

        public Guid ConfigMountPointId => Guid.Empty;

        public string ConfigPlace => string.Empty;

        public Guid LocaleConfigMountPointId => Guid.Empty;

        public string LocaleConfigPlace => string.Empty;

        public int Precedence => -1;

        [Obsolete]
        public IReadOnlyDictionary<string, ICacheParameter> CacheParameters => null;

        public IReadOnlyList<ITemplateParameter> Parameters => null;

        public Guid HostConfigMountPointId => Guid.Empty;

        public string HostConfigPlace => string.Empty;

        public string ThirdPartyNotices => string.Empty;

        public IReadOnlyDictionary<string, IBaselineInfo> BaselineInfo => null;

        public bool HasScriptRunningPostActions { get; set; }

        // These were added for the VS2022 support
        public IReadOnlyDictionary<string, string> TagsCollection => _tags;

        public string MountPointUri { get; set; }

        public IReadOnlyList<string> ShortNameList { get; set; }

        public IParameterDefinitionSet ParameterDefinitions { get; }

        public IReadOnlyList<Guid> PostActions { get; }

        public IReadOnlyList<TemplateConstraintInfo> Constraints { get; }

        public bool PreferDefaultName { get; }
    }
}
