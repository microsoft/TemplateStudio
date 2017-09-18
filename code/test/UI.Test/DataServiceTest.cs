// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;

using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.NewProject;

using Xunit;

namespace UI.Test
{
    public class DataServiceTest : IClassFixture<TemplatesFixture>
    {
        private TemplatesFixture _fixture;

        public DataServiceTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture("C#");
        }

        [Fact]
        public void LoadProjectTypes()
        {
            ObservableCollection<MetadataInfoViewModel> projectTypes = new ObservableCollection<MetadataInfoViewModel>();
            Assert.True(DataService.LoadProjectTypes(projectTypes));
        }

        [Fact]
        public void LoadFrameworks()
        {
            ObservableCollection<MetadataInfoViewModel> frameworks = new ObservableCollection<MetadataInfoViewModel>();
            Assert.True(DataService.LoadFrameworks(frameworks, "Blank"));
        }
    }
}
