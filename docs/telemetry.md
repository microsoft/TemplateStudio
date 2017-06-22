# Windows Template Studio Telemetry

Windows Template Studio wizard is ready to gather basic diagnostics telemetry and usage data through [Application Insights](https://azure.microsoft.com/en-us/services/application-insights/).

The class [`Diagnostics`](code/src/Core/Diagnostics)/[`TelemetryService`](../code/src/Core/Diagnostics/TelemetryService.cs), within the [Core](../code/src/Core) assembly, isolates the telemetry service implementation details and offer a smooth and easy way to invoke telemetry events. Just in case the telemetry backend needs / wants to be replaced, all the changes will be located at this class.

## Trends

Please head to our [Telemetry Data](telemetryData.md) where we show trends from the gathered telemetry.

## Telemetry Gathered

The Wizard for Windows Template Studio collects basic diagnostics telemetry and usage data:

* **Diagnostics telemetry**: unhandled exceptions happened while running the Wizard. This enable the exception tracking and analysis.
* **Usage telemetry**: including wizard usage and user selections.

## Usage telemetry collected

Through Application Insights API, telemetry events are collected to track gather basic information regarding Windows Template Studio extension usage. The following table describes the Telemetry Events we collect:

|Event Name Tracked |Notes |
|:-------------:|:-----|
| **Session** | Tracked every time the user starts a new session with the Windows Template Studio Wizard, that is, every time the Wizard is instantiated for the first time within a Visual Studio session.|
| **Wizard** | Tracked every time the Wizard has been executed recording the wizard finalization status.|
| **ProjectGen** | After a project is generated, this event is tracked to collect detailed *project information* (like generation status, project type, framework, template Name, generation engine info) and *project metrics* (as generation duration, # of pages and # of features).|
| **PageGen** | After a project is generated, this event is tracked for each page added to the project. It collects the *page information* (page template, framework and generation engine info).|
| **FeatureGen** | After a project is generated, this event is tracked for each developer feature included in the project. It collects the *feature information* (template, framework and generation engine info).|

## Telemetry Configuration

The [TelemetryService](../code/src/Core/Diagnostics/TelemetryService.cs) class is based on Application Insights API. The Application Insights telemetry backend requires a telemetry instrumentation key to be able to track telemetry. If you want to track your own telemetry, you will need your own instrumentation key, obtain one by creating an [Application Insights](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-asp-net) instance in your Azure account, if you don't have an Azure account there are different options to [create one for free](https://azure.microsoft.com/en-us/free/).

The instrumentation key is setup through the wizard configuration. The default configuration values are those that are defined directly in the code:

``` csharp

public class Configuration
{
    ...
    //Set your Application Insights telemetry instrumentation key here (configure it in a WindowsTemplateStudio.config.json located in the working folder).
    public string RemoteTelemetryKey { get; set; } = "<SET_YOUR_OWN_KEY>";
    ...
}
```

You can setup your key in the `Configuration` class or provide it through a configuration file. The following section describe the configuration overrides.

## Configuration overrides

Default configuration values can be overridden using two different mechanisms:

1. *Predetermined configuration file*: if a configuration with the name **WindowsTemplateStudio.config.json** is found in the path where the Core assembly is located while running, the configuration values are loaded from the file. Partial configuration is allowed so you don't need to specify all configuration values, just those you want to modify / update.
1. *Redirected config file*: This works only for testing purposes. You can modify the configuration path and filename by specifying an appSetting in the App.Config for the Unit Tests or the VsEmulator app. The appSetting must be specified as follows:

``` xml

<add key="JsonConfigFile" value="MyCustomFile.config.json.secret" />

```

If you add the ".secret" extension to your configuration file, it will not be pushed to the Github repo (an exception for .secret files is in this repo .gitignore file).

As mentioned, the configuration files allow you to define only the configuration elements you want to override. Check the following sample content which overrides the RemoteTelemetryKey and DiagnosticsTraceLevel settings:

``` json

WindowsTemplateStudio.config.json
{
  "RemoteTelemetryKey": "your-key",
  "DiagnosticsTraceLevel": "Warning"
}

```
