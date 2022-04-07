// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using ApiAnalysis;
using Microsoft.TemplateEngine.Abstractions;
using Newtonsoft.Json;

namespace TemplateValidator
{
    /// <summary>
    /// This class is used as a reference for ITemplateInfo. We don't deserialize to it directly but the Templating library does.
    /// This class also stores attribute information used by the analyzer.
    /// For more about the contents of a template file see https://github.com/dotnet/templating/wiki/%22Runnable-Project%22-Templates#identity-optional
    /// </summary>
    public class ValidationTemplateInfo : ITemplateInfo
    {
        [JsonProperty("$schema")]
        public string Schema { get; set; }

        [ApiAnalysisValidValues("Microsoft", "Microsoft Community", "Laurent Bugnion", "Laurent Bugnion + Community Contribution", "Nigel Sampson", "Matt Lacey")]
        public string Author { get; set; }

        [ApiAnalysisOptional]
        public string Description { get; set; }

        public IReadOnlyList<string> Classifications { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public string DefaultName { get; set; }

        public string Identity { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public Guid GeneratorId { get; set; }

        [ApiAnalysisOptional]
        public string GroupIdentity { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public int Precedence { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        // This is how the interface defines the Tags property
        [JsonIgnore]
        [Obsolete]
        public IReadOnlyDictionary<string, ICacheTag> Tags { get; }

        // We just use strings for tags. The template engine uses a converter but this is fine for testing purposes
        [ApiAnalysisMandatoryKeys("language", "type", "ts.type", "ts.platform")]
        [ApiAnalysisOptionalKeys("ts.displayOrder", "ts.compositionOrder", "ts.frontendframework", "ts.backendframework", "ts.projecttype", "ts.version", "ts.genGroup", "ts.rightClickEnabled", "ts.compositionFilter", "ts.licenses", "ts.group", "ts.multipleInstance", "ts.dependencies", "ts.requirements", "ts.exclusions", "ts.defaultInstance", "ts.export.baseclass", "ts.export.setter", "ts.isHidden", "ts.telemName", "ts.outputToParent", "ts.isGroupExclusiveSelection", "ts.requiredVsWorkload", "ts.requiredVersions", "ts.export.configtype", "ts.export.configvalue", "ts.export.commandclass", "ts.export.pagetype", "ts.export.canExecuteChangedMethodName", "ts.export.onNavigatedToParams", "ts.export.onNavigatedFromParams", "ts.appmodel")]
        [JsonProperty("tags")]
        public IReadOnlyDictionary<string, string> TagsCollection { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        [Obsolete]
        public IReadOnlyDictionary<string, ICacheParameter> CacheParameters { get; set; }

        [ApiAnalysisOptional]
        public IReadOnlyList<ITemplateParameter> Parameters { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public Guid ConfigMountPointId { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public string ConfigPlace { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public Guid LocaleConfigMountPointId { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public string LocaleConfigPlace { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public Guid HostConfigMountPointId { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public string HostConfigPlace { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public string ThirdPartyNotices { get; set; }

        [ApiAnalysisOptional]
        public IReadOnlyDictionary<string, IBaselineInfo> BaselineInfo { get; set; }

        [ApiAnalysisOptional]
        public List<PrimaryOutput> PrimaryOutputs { get; set; }

        [ApiAnalysisOptional]
        public IReadOnlyDictionary<string, SymbolInfo> Symbols { get; set; }

        [ApiAnalysisOptional]
        public List<PostActionInfo> PostActions { get; set; }

        public string SourceName { get; set; }

        [ApiAnalysisOptional]
        public bool PlaceholderFilename { get; set; }

        [ApiAnalysisOptional]
        public bool PreferNameDirectory { get; set; }

        [ApiAnalysisOptional]
        public List<string> Guids { get; set; }

        [JsonIgnore]
        public bool HasScriptRunningPostActions { get; set; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public string MountPointUri { get; }

        [ApiAnalysisShouldNotBeInJson("We don't use it.")]
        public IReadOnlyList<string> ShortNameList { get; }
    }
}
