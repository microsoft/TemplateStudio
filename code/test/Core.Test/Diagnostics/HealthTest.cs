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
    public class TestHealthsWriter : IHealthWriter
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
    }
    public class HealthTest
    {
        [Fact]
        public async Task UsageAsync()
        {
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

            await AppHealth.Current.Telemetry.TrackTemplateGeneratedAsync("TemplateName", "TemplateFramework", "TemplateType");
        }

        [Fact]
        public async Task AddAdditionalWriterAsync()
        {
            TestHealthsWriter newWriter = new TestHealthsWriter();

            await AppHealth.Current.Error.TrackAsync("WillNotBeRegisteredYet");

            AppHealth.Current.AddWriter(newWriter); 
            await AppHealth.Current.Error.TrackAsync("WillBeRegistered");

            Exception exToTest = new Exception("RegisteredEx");
            await AppHealth.Current.Exception.TrackAsync(exToTest);

            Assert.DoesNotContain("Error;WillNotBeRegisteredYet;", newWriter.Events);
            Assert.Contains("Error;WillBeRegistered;", newWriter.Events);
            Assert.Contains<Exception>(exToTest, newWriter.Exceptions);
        }

        [Fact]
        public async Task VerifyTraceLevelChangeAsync()
        {
            TestHealthsWriter newWriter = new TestHealthsWriter();

            Configuration config = Configuration.Current;
            config.DiagnosticsTraceLevel = TraceEventType.Warning;

            Configuration.UpdateCurrentConfiguration(config);

            AppHealth.Current.AddWriter(newWriter);

            await AppHealth.Current.Verbose.TrackAsync("VerboseShouldNotBeRegistered");
            await AppHealth.Current.Info.TrackAsync("InfoShouldNotBeRegistered");
            await AppHealth.Current.Warning.TrackAsync("WarningMustBeRegistered");
            await AppHealth.Current.Error.TrackAsync("ErrorMustBeRegistered");

            Exception exToTest = new Exception("ExceptionMustBeRegistered");
            await AppHealth.Current.Exception.TrackAsync(exToTest);

            Assert.DoesNotContain("Verbose;VerboseShouldNotBeRegistered;", newWriter.Events);
            Assert.DoesNotContain("Info;InfoShouldNotBeRegistered;", newWriter.Events);
            Assert.Contains("Warning;WarningMustBeRegistered;", newWriter.Events);
            Assert.Contains("Error;ErrorMustBeRegistered;", newWriter.Events);
            Assert.Contains<Exception>(exToTest, newWriter.Exceptions);
        }
    }
}
