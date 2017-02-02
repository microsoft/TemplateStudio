using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Diagnostics;


namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TestHealthWriter : IHealthWriter
    {
        public List<string> Events = new List<string>();
        public List<Exception> Exceptions = new List<Exception>();

        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {
            await Task.Run(()=>Events.Add($"{eventType.ToString()};{message};{ex?.ToString()}"));
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            await Task.Run(() => Exceptions.Add(ex));
        }

        public bool AllowMultipleInstances()
        {
            return true;
        }
    }
    public class HealthTest
    {
        TestHealthWriter testWriter=null;
        public HealthTest()
        {
            testWriter = new TestHealthWriter();
            AppHealth.Current.AddWriter(testWriter);
        }
        [Fact]
        public async Task UsageAsync()
        {
            int initialEvents= testWriter.Events.Count;
            int initialExceptions = testWriter.Exceptions.Count;

            //Instance with default configuration
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

            //Check events flow to the TestHealthWriter
            Assert.True(initialEvents < testWriter.Events.Count);
            Assert.True(initialExceptions < testWriter.Exceptions.Count);
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

                Assert.DoesNotContain("Verbose;VerboseShouldNotBeRegistered;", testWriter.Events);
                Assert.DoesNotContain("Info;InfoShouldNotBeRegistered;", testWriter.Events);
                Assert.Contains("Warning;WarningMustBeRegistered;", testWriter.Events);
                Assert.Contains("Error;ErrorMustBeRegistered;", testWriter.Events);
                Assert.Contains<Exception>(exToTest, testWriter.Exceptions);
            }
            finally
            {
                //Restore previous config
                Configuration.Current.DiagnosticsTraceLevel = previousLevel;
            }
        }
    }
}
