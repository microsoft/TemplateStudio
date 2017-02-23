using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    public class NamingTest
    {
        [Fact]
        public void Infer_Existing()
        {
            var existing = new string[] { "Blank" };
            var result = Naming.Infer(existing, "Blank");

            Assert.Equal("Blank1", result);
        }

        [Fact]
        public void Infer_Reserved()
        {
            var existing = new string[] { };
            var result = Naming.Infer(existing, "WebView");

            Assert.Equal("WebView1", result);
        }

        [Fact]
        public void Infer_Clean()
        {
            var existing = new string[] { };
            var result = Naming.Infer(existing, "Blank$Page");

            Assert.Equal("BlankPage", result);
        }

        [Fact]
        public void Validate()
        {
            var existing = new string[] { };
            var result = Naming.Validate(existing, "Blank1");

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_Empty()
        {
            var existing = new string[] { };
            var result = Naming.Validate(existing, "");

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.Empty, result.ErrorType);
        }

        [Fact]
        public void Validate_Existing()
        {
            var existing = new string[] { "Blank" };
            var result = Naming.Validate(existing, "Blank");

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.AlreadyExists, result.ErrorType);
        }

        [Fact]
        public void Validate_BadFormat_InvalidChars()
        {
            var existing = new string[] { };
            var result = Naming.Validate(existing, "Blank;");

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }

        [Fact]
        public void Validate_BadFormat_StartWithNumber()
        {
            var existing = new string[] { };
            var result = Naming.Validate(existing, "1Blank");

            Assert.False(result.IsValid);
            Assert.Equal(ValidationErrorType.BadFormat, result.ErrorType);
        }
    }

}
