// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using LibGit2Sharp;

namespace Localization
{
    public class ValidateLocalizableExtractor
    {
        private string _repository;
        private string _extractorParameter;
        private ExtractorMode _extractorMode;

        public ValidateLocalizableExtractor(string repository, ExtractorMode extractorMode, string extractorParameter)
        {
            _repository = repository;
            _extractorMode = extractorMode;
            _extractorParameter = extractorParameter;
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
               return repo.Commits.QueryBy(file, filter).Any();
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
