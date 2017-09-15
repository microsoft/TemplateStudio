// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace Localization
{
    public class ValidateLocalizableExtractor
    {
        private string _repository;
        private string _tagName;
        private IEnumerable<string> _validateFiles;
        private List<string> _generalFiles = new List<string>
        {
            Routes.VsixValidatePath,
            Routes.ProjectTemplateFileNameValidate,
            Routes.RelayCommandFileNameValidate,
            Routes.VspackageFileNameValidate,
            Routes.WtsProjectTypesValidate,
            Routes.WtsFrameworksValidate
        };

        public ValidateLocalizableExtractor(string repository, string tagName)
        {
            _repository = repository;
            _tagName = tagName;
            _validateFiles = string.IsNullOrEmpty(tagName) ? _generalFiles : CheckModifiedFiles(_generalFiles).ToList();
        }

        public bool ValidVisxFile() => _validateFiles.Contains(Routes.VsixValidatePath);
        public bool ValidProjectTemplateFile() => _validateFiles.Contains(Routes.ProjectTemplateFileNameValidate);
        public bool ValidRelayCommandeFile() => _validateFiles.Contains(Routes.RelayCommandFileNameValidate);
        public bool ValidVspackageFile() => _validateFiles.Contains(Routes.VspackageFileNameValidate);
        public bool ValidWtsProjectTypes() => _validateFiles.Contains(Routes.WtsProjectTypesValidate);
        public bool ValidWtsFrameworks() => _validateFiles.Contains(Routes.WtsFrameworksValidate);

        private IEnumerable<string> CheckModifiedFiles(IEnumerable<string> files)
        {
            using (var repo = new Repository(_repository))
            {
                var filter = ConfigureFilter(repo);
                foreach (var file in files)
                {
                    if (repo.Commits.QueryBy(file, filter).Any())
                    {
                        yield return file;
                    }
                }
            }
        }

        public bool CheckModifiedFile(string file)
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

        private CommitFilter ConfigureFilter(Repository repo)
        {
            return new CommitFilter
            {
                ExcludeReachableFrom = repo.Tags[_tagName].Target.Sha, // SHA from tag
                IncludeReachableFrom = repo.Head.Tip.Sha // SHA from last commit
            };
        }
    }
}
