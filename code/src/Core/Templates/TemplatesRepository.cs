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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Locations;

using Newtonsoft.Json;

namespace Microsoft.Templates.Core
{
    public class TemplatesRepository
    {
        private const string Separator = "|";
        private const string LicensesPattern = @"\[(?<text>.*?)\]\((?<url>.*?)\)\" + Separator + "?";
        private const string Catalog = "_catalog";
        private static readonly string[] SupportedIconTypes = new string[] { ".jpg", ".jpeg", ".png", ".xaml" };

        public TemplatesSynchronization Sync { get; private set; }
        public string WizardVersion { get; private set; }
        public string CurrentContentFolder { get => Sync?.CurrentContentFolder; }
        public string TemplatesVersion { get => Sync.CurrentContentVersion?.ToString() ?? string.Empty; }

        public TemplatesRepository(TemplatesSource source, Version wizardVersion)
        {
            WizardVersion = wizardVersion.ToString();
            Sync = new TemplatesSynchronization(source, wizardVersion);
        }

        public async Task SynchronizeAsync(bool force = false)
        {
            await Sync.Do(force);
        }

        public IEnumerable<ITemplateInfo> GetAll()
        {
            var queryResult = CodeGen.Instance.Cache.List(false, WellKnownSearchFilters.LanguageFilter("C#"));

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

        ////public IEnumerable<ITemplateInfo> GetDependencies(ITemplateInfo ti)
        ////{
        ////    return ti.GetDependencyList()
        ////                .Select(d => Find(t => t.Identity == d));
        ////}

        public ITemplateInfo Find(Func<ITemplateInfo, bool> predicate)
        {
            return GetAll()
                        .FirstOrDefault(predicate);
        }

        public IEnumerable<MetadataInfo> GetProjectTypes()
        {
            return GetMetadataInfo("projectTypes");
        }

        public IEnumerable<MetadataInfo> GetFrameworks()
        {
            return GetMetadataInfo("frameworks");
        }

        private IEnumerable<MetadataInfo> GetMetadataInfo(string type)
        {
            var folderName = Path.Combine(Sync.CurrentContentFolder, Catalog);

            if (!Directory.Exists(folderName))
            {
                return Enumerable.Empty<MetadataInfo>();
            }

            var metadataFile = Path.Combine(folderName, $"{type}.json");
            var metadata = JsonConvert.DeserializeObject<List<MetadataInfo>>(File.ReadAllText(metadataFile));

            metadata.ForEach(m => SetMetadataDescription(m, folderName, type));
            metadata.ForEach(m => SetMetadataIcon(m, folderName, type));
            metadata.ForEach(m => m.MetadataType = type);
            metadata.ForEach(m => SetLicenseTerms(m));

            return metadata.OrderBy(m => m.Order);
        }

        private void SetLicenseTerms(MetadataInfo metadataInfo)
        {
            if (!string.IsNullOrWhiteSpace(metadataInfo.Licenses))
            {
                var result = new List<TemplateLicense>();
                var licensesMatches = Regex.Matches(metadataInfo.Licenses, LicensesPattern);

                for (int i = 0; i < licensesMatches.Count; i++)
                {
                    var m = licensesMatches[i];

                    if (m.Success)
                    {
                        result.Add(new TemplateLicense
                        {
                            Text = m.Groups["text"].Value,
                            Url = m.Groups["url"].Value
                        });
                    }
                }

                metadataInfo.LicenseTerms = result;
            }
        }

        private static void SetMetadataDescription(MetadataInfo mInfo, string folderName, string type)
        {
            var descriptionFile = Path.Combine(folderName, type, $"{mInfo.Name}.md");
            if (File.Exists(descriptionFile))
            {
                mInfo.Description = File.ReadAllText(descriptionFile);
            }
        }

        private static void SetMetadataIcon(MetadataInfo mInfo, string folderName, string type)
        {
            var iconFile = Directory
                            .EnumerateFiles(Path.Combine(folderName, type))
                            .Where(f => SupportedIconTypes.Contains(Path.GetExtension(f)))
                            .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(mInfo.Name, StringComparison.OrdinalIgnoreCase));

            if (File.Exists(iconFile))
            {
                mInfo.Icon = iconFile;
            }
        }

        ////public ITemplateInfo GetLayoutTemplate(LayoutItem item, string framework)
        ////{
        ////    return Find(t => t.GroupIdentity == item.templateGroupIdentity && t.GetFrameworkList().Any(f => f.Equals(framework, StringComparison.OrdinalIgnoreCase)));
        ////}
    }
}
