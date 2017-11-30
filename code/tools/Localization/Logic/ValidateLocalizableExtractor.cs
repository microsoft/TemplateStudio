// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using LibGit2Sharp;
using Localization.Options;

namespace Localization
{
    public class ValidateLocalizableExtractor
    {
        private string _repository;
        private string _extractorParameter;
        private ExtractorMode _extractorMode;

        public ValidateLocalizableExtractor(ExtractOptions options)
        {
            _repository = options.SourceDirectory;
            _extractorMode = options.ExtractorMode;

            switch (options.ExtractorMode)
            {
                case ExtractorMode.Commit:
                    _extractorParameter = options.CommitSha;
                    break;
                case ExtractorMode.TagName:
                    _extractorParameter = options.TagName;
                    break;
                default:
                    _extractorParameter = string.Empty;
                    break;
            }
        }

        internal bool HasChanges(string file)
        {
            if (_extractorMode == ExtractorMode.All || string.IsNullOrEmpty(_extractorParameter))
            {
                return true;
            }

            using (var repo = new Repository(_repository))
            {
                var filter = ConfigureFilter(repo);

                var referenceCommitDate = repo.Lookup<Commit>(filter.ExcludeReachableFrom.ToString()).Author.When;

                var commits = repo.Commits.QueryBy(file, filter).Where(c => c.Commit.Author.When > referenceCommitDate);

                return commits.Any();
            }
        }

        private CommitFilter ConfigureFilter(Repository repository)
        {
            var excludeReachableFrom = _extractorMode == ExtractorMode.Commit
                ? _extractorParameter
                : repository.Tags[_extractorParameter].Target.Sha; // SHA from tag

            return new CommitFilter
            {
                ExcludeReachableFrom = excludeReachableFrom,
                IncludeReachableFrom = repository.Head.Tip.Sha // SHA from last commit
            };
        }
    }
}
