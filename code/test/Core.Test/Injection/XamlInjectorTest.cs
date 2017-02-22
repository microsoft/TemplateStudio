using Microsoft.Templates.Core.Injection.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Microsoft.Templates.Core.Test.Injection
{
    public class XamlInjectorTest
    {
        private const string Xaml = @"<Application
                                        x:Class=""BlankProject.App""
                                        xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                        xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                        RequestedTheme=""Light"">

                                        <Application.Resources>
                                            <ResourceDictionary>
                                            </ResourceDictionary>
                                        </Application.Resources>
                                    </Application>";

        private const string EspectedXaml = @"<Application
                                                x:Class=""BlankProject.App""
                                                xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                                xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                                RequestedTheme=""Light""
                                                xmlns:local1=""using:TestProject1""
                                                xmlns:local2=""using:TestProject2"">

                                                <Application.Resources>
                                                    <ResourceDictionary>
                                                        <local1:ViewModelLocator x:Key=""Locator1"" />
                                                        <local2:ViewModelLocator x:Key=""Locator2"" />
                                                        <local1:ViewModelLocator2 x:Key=""Locator3"" />
                                                    </ResourceDictionary>
                                                </Application.Resources>
                                            </Application>";

        [Fact]
        public void Inject()
        {
            var config = new XamlInjectorConfig
            {
                elements = new Element[]
                {
                    new Element
                    {
                        path = "/Application/Application.Resources/ResourceDictionary",
                        content = "<local1:ViewModelLocator xmlns:local1=\"using:TestProject1\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Key=\"Locator1\" />"
                    },
                    new Element
                    {
                        path = "/Application/Application.Resources/ResourceDictionary",
                        content = "<local2:ViewModelLocator xmlns:local2=\"using:TestProject2\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Key=\"Locator2\" />"
                    },
                    new Element
                    {
                        path = "/Application/Application.Resources/ResourceDictionary",
                        content = "<local1:ViewModelLocator2 xmlns:local1=\"using:TestProject1\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Key=\"Locator3\" />"
                    },
                }
            };
            var target = new XamlInjector(config);

            var result = target.Inject(Xaml);

            Assert.NotNull(result);
            Assert.Equal(XElement.Parse(EspectedXaml).ToString(), XElement.Parse(result).ToString());
        }
    }
}
