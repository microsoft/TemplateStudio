// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Test.TestFakes;

using Xunit;

namespace Microsoft.Templates.Core.Test.Locations
{
    [Trait("ExecutionSet", "Minimum")]

    public class RemoteTemplateSourceTests
    {
        [Fact]
        public async Task TestRemoteSource_AcquireAsync()
        {
            var platform = Platforms.Uwp;
            var language = ProgrammingLanguages.CSharp;

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates");

            RemoteTemplatesSource rts = new RemoteTemplatesSource(platform, language, path, new TestDigitalSignatureService());
            CancellationTokenSource cts = new CancellationTokenSource();

            await rts.LoadConfigAsync(cts.Token);
            var package = rts.Config.Latest;

            await rts.AcquireAsync(package, cts.Token);

            string acquiredContentFolder = package.LocalPath;

            Assert.NotNull(acquiredContentFolder);

            // Ensure package is not downloaded again if already downloaded
            await rts.AcquireAsync(package, cts.Token);
            Assert.True(acquiredContentFolder == package.LocalPath);

            // Reset localPath and ensure it is acquired again
            if (Directory.Exists(Path.GetDirectoryName(package.LocalPath)))
            {
                Directory.Delete(Path.GetDirectoryName(package.LocalPath), true);
            }

            await rts.AcquireAsync(package, cts.Token);

            Assert.True(package.LocalPath != acquiredContentFolder);

            if (Directory.Exists(Path.GetDirectoryName(package.LocalPath)))
            {
                Directory.Delete(Path.GetDirectoryName(package.LocalPath), true);
            }
        }

        [Fact]
        public async Task TestRemoteSource_AcquireCancelAsync()
        {
            var platform = Platforms.Uwp;
            var language = ProgrammingLanguages.CSharp;

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates");

            RemoteTemplatesSource rts = new RemoteTemplatesSource(platform, language, path, new TestDigitalSignatureService());
            CancellationTokenSource cts = new CancellationTokenSource();

            await rts.LoadConfigAsync(cts.Token);
            var package = rts.Config.Latest;

            cts.CancelAfter(TimeSpan.FromMilliseconds(20));
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await rts.AcquireAsync(package, cts.Token);
            });

            string acquiredContentFolder = package.LocalPath;

            Assert.Null(acquiredContentFolder);

            // Ensure package is downloaded again if cancelled
            await rts.AcquireAsync(package, cts.Token);
            Assert.True(acquiredContentFolder != package.LocalPath);

            if (Directory.Exists(Path.GetDirectoryName(package.LocalPath)))
            {
                Directory.Delete(Path.GetDirectoryName(package.LocalPath), true);
            }
        }

        [Fact]
        public async Task TestRemoteSource_GetContentAsync()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(TemplatePackageTests).Assembly.CodeBase).LocalPath);
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                var platform = Platforms.Uwp;
                var language = ProgrammingLanguages.CSharp;

                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates");

                RemoteTemplatesSource rts = new RemoteTemplatesSource(platform, language, path, new TestDigitalSignatureService());
                CancellationTokenSource cts = new CancellationTokenSource();
                await rts.LoadConfigAsync(cts.Token);
                var package = rts.Config.Latest;

                await rts.AcquireAsync(package, cts.Token);
                var contentInfo = await rts.GetContentAsync(package, testDir, cts.Token);

                Assert.True(Directory.Exists(contentInfo.Path));
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Fact]
        public async Task TestRemoteSource_GetContentCancelAsync()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(TemplatePackageTests).Assembly.CodeBase).LocalPath);
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                var platform = Platforms.Uwp;
                var language = ProgrammingLanguages.CSharp;

                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates");

                RemoteTemplatesSource rts = new RemoteTemplatesSource(platform, language, path, new TestDigitalSignatureService());
                CancellationTokenSource cts = new CancellationTokenSource();
                await rts.LoadConfigAsync(cts.Token);
                var package = rts.Config.Latest;

                await rts.AcquireAsync(package, cts.Token);

                cts.CancelAfter(TimeSpan.FromSeconds(1));
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                {
                    await rts.GetContentAsync(package, testDir, cts.Token);
                });

                // Ensure package is extracted again if cancelled
                var contentInfo = await rts.GetContentAsync(package, testDir, cts.Token);

                Assert.True(Directory.Exists(contentInfo.Path));
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Fact]
        public async Task TestRemoteSource_GetContentNoPackageAsync()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(TemplatePackageTests).Assembly.CodeBase).LocalPath);
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                var platform = Platforms.Uwp;
                var language = ProgrammingLanguages.CSharp;
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates");

                RemoteTemplatesSource rts = new RemoteTemplatesSource(platform, language, path, new TestDigitalSignatureService());
                CancellationTokenSource cts = new CancellationTokenSource();
                await rts.LoadConfigAsync(cts.Token);
                var package = rts.Config.Latest;

                var contentInfo = await rts.GetContentAsync(package, testDir, cts.Token);

                Assert.Null(contentInfo);
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Fact]
        public async Task TestRemoteSource_GetLocalContentAsync()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(TemplatePackageTests).Assembly.CodeBase).LocalPath);
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                var platform = Platforms.Uwp;
                var language = ProgrammingLanguages.CSharp;

                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InstalledTemplates");

                RemoteTemplatesSource rts = new RemoteTemplatesSource(platform, language, path, new TestDigitalSignatureService());
                CancellationTokenSource cts = new CancellationTokenSource();

                var packageFile = Path.GetFullPath(@".\Packaging\MsSigned\Templates.mstx");

                var package = new TemplatesPackageInfo()
                {
                    Name = Path.GetFileName(packageFile),
                    LocalPath = packageFile,
                    WizardVersions = new List<string>() { GenContext.GetWizardVersionFromAssembly() },
                };

                var contentInfo = await rts.GetContentAsync(package, testDir, cts.Token);

                Assert.True(Directory.Exists(contentInfo.Path));
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }
    }
}
