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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Locations;

using Newtonsoft.Json;

namespace Microsoft.Templates.Core
{
    public class TemplatesRepository
    {
        private const string ProjectTypes = "Projects";
        private const string Frameworks = "Frameworks";
        private const string Metadata = ".metadata";

        public TemplatesSynchronization Sync { get; private set; }

        public string CurrentContentFolder { get => Sync?.CurrentContentFolder; }

        public TemplatesRepository(TemplatesSource source)
        {
            Sync = new TemplatesSynchronization(source);
        }


        public string GetVersion()
        {
            return Sync.CurrentContentVersion?.ToString();
        }

        public async Task SynchronizeAsync(bool force = false)
        {
            await Sync.Do(force);
        }

        public IEnumerable<ITemplateInfo> GetAll()
        {
            var queryResult = CodeGen.Instance.Creator.List(false, WellKnownSearchFilters.LanguageFilter("C#"));

            return queryResult
                        .Where(r => r.IsMatch)
                        .Select(r => r.Info)
                        .ToList();
        }

        public IEnumerable<ITemplateInfo> Get(Func<ITemplateInfo, bool> predicate)
        {
            return GetAll()
                        .Where(predicate);
        }

        public IEnumerable<ITemplateInfo> GetDependencies(ITemplateInfo ti)
        {
            return ti.GetDependencyList().Select(d => GetAll().FirstOrDefault(t => t.Identity == d));
        }


        public ITemplateInfo Find(Func<ITemplateInfo, bool> predicate)
        {
            return GetAll()
                        .FirstOrDefault(predicate);
        }


        public MetadataInfo GetProjectTypeInfo(string projectType)
        {
            return GetMetadataInfo(Path.Combine(Sync.CurrentContentFolder, ProjectTypes, projectType, Metadata));
        }

        public MetadataInfo GetFrameworkTypeInfo(string fxType)
        {
            return GetMetadataInfo(Path.Combine(Sync.CurrentContentFolder, Frameworks, fxType, Metadata));
        }

        private MetadataInfo GetMetadataInfo(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                return null;
            }

            string descriptionFile = Path.Combine(folderName, $"info.json");
            var metadata = JsonConvert.DeserializeObject<MetadataInfo>(File.ReadAllText(descriptionFile));

            var metadataInfo = new MetadataInfo()
            {
                DisplayName = metadata.DisplayName,
                Description = metadata.Description,
                Icon = Path.Combine(folderName, $"icon.png")
            };
            return metadataInfo;
        }
    }
}
