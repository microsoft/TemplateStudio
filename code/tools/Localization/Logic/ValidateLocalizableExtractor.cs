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
        private string _tagName;

        public ValidateLocalizableExtractor(string repository, string tagName)
        {
            _repository = repository;
            _tagName = tagName;
        }

        internal bool HasChanges(string file)
        {
            if (string.IsNullOrEmpty(_tagName))
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
            return new CommitFilter
            {
                ExcludeReachableFrom = repository.Tags[_tagName].Target.Sha, // SHA from tag
                IncludeReachableFrom = repository.Head.Tip.Sha // SHA from last commit
            };
        }
    }
}
