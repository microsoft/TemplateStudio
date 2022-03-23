// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Xunit;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    [Trait("Group", "Minimum")]
    public class HealthTest
    {
        private TestHealthWriter _testWriter = null;

        public HealthTest()
        {
            _testWriter = new TestHealthWriter();
            AppHealth.Current.AddWriter(_testWriter);
        }

        [Fact]
        public async Task UsageAsync()
        {
            int initialEvents = _testWriter.Events.Count;
            int initialExceptions = _testWriter.Exceptions.Count;

            // Instance with default configuration
            await AppHealth.Current.Verbose.TrackAsync("VerboseMessage");
            await AppHealth.Current.Verbose.TrackAsync("VerboseMesssage with exception", new Exception("VerboseExceptionInfo"));

            await AppHealth.Current.Info.TrackAsync("InfoMesssage");
            await AppHealth.Current.Info.TrackAsync("InfoMesssage with exception", new Exception("InfoExceptionInfo"));

            await AppHealth.Current.Warning.TrackAsync("InfoWarning");
            await AppHealth.Current.Warning.TrackAsync("InfoWarning with exception", new Exception("WarningExceptionInfo"));

            await AppHealth.Current.Error.TrackAsync("InfoError");
            await AppHealth.Current.Error.TrackAsync("InfoError with exception", new Exception("ErrorExceptionInfo"));

            await AppHealth.Current.Exception.TrackAsync(new Exception("ExceptionTracked"));
            await AppHealth.Current.Exception.TrackAsync(new Exception("ExceptionTrackedWithAddtionalInfo"), "AddtionalInfo");

            // Check events flow to the TestHealthWriter
            Assert.True(initialEvents < _testWriter.Events.Count);
            Assert.True(initialExceptions < _testWriter.Exceptions.Count);
        }

        [Fact]
        public async Task AddAdditionalWriterAsync()
        {
            TestHealthWriter newWriter = new TestHealthWriter();

            await AppHealth.Current.Error.TrackAsync("WillNotBeRegisteredYet");

            AppHealth.Current.AddWriter(newWriter);

            await AppHealth.Current.Error.TrackAsync("WillBeRegistered");

            Assert.DoesNotContain("Error;WillNotBeRegisteredYet;", newWriter.Events);
            Assert.Contains("Error;WillBeRegistered;", newWriter.Events);
        }

        [Fact]
        public async Task VerifyTraceLevelChangeAsync()
        {
            TraceEventType previousLevel = Configuration.Current.DiagnosticsTraceLevel;

            try
            {
                Configuration.Current.DiagnosticsTraceLevel = TraceEventType.Warning;

                await AppHealth.Current.Verbose.TrackAsync("VerboseShouldNotBeRegistered");
                await AppHealth.Current.Info.TrackAsync("InfoShouldNotBeRegistered");
                await AppHealth.Current.Warning.TrackAsync("WarningMustBeRegistered");
                await AppHealth.Current.Error.TrackAsync("ErrorMustBeRegistered");

                Exception exToTest = new Exception("ExceptionMustBeRegistered");

                await AppHealth.Current.Exception.TrackAsync(exToTest);

                Assert.DoesNotContain("Verbose;VerboseShouldNotBeRegistered;", _testWriter.Events);
                Assert.DoesNotContain("Info;InfoShouldNotBeRegistered;", _testWriter.Events);
                Assert.Contains("Warning;WarningMustBeRegistered;", _testWriter.Events);
                Assert.Contains("Error;ErrorMustBeRegistered;", _testWriter.Events);
                Assert.Contains<Exception>(exToTest, _testWriter.Exceptions);
            }
            finally
            {
                // Restore previous config
                Configuration.Current.DiagnosticsTraceLevel = previousLevel;
            }
        }
    }
}
