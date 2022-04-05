# Template Studio Telemetry

*Template Studio* relies on Core Template Studio for telemetry. For info about implementation, telemetry collected and configuration see [Core Template Studio Telemetry](https://github.com/microsoft/CoreTemplateStudio/blob/main/docs/telemetry.md).

## Trends

Please head to our [Telemetry Data](telemetryData.md) where we show trends from the gathered telemetry.

## Usage telemetry collected

For events related to session and project generation see [Core Template Studio -  Usage telemetry collected](https://github.com/microsoft/CoreTemplateStudio/blob/main/docs/telemetry.md#usage-telemetry-collected). On the *Template Studio* Wizard we additionally collect the following events:

|Event Name Tracked |Notes |
|:-------------:|:-----|
| **Wizard** | Tracked every time the Wizard has been executed recording the wizard finalization status.|
| **EditSummaryItem** | Tracked every time the user selection is edited using the right side bar.|


## Learn more

- [Getting started with the WinTS codebase](./getting-started-developers.md)
- [Understanding and authoring Templates](./templates.md)
- [Ensuring generated code is accessible](./accessibility.md)
- [All docs](./readme.md)
