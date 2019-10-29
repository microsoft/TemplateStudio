# Accessibility checklist

Here we provide a checklist you can use to ensure that your pull request meets the accessibility requirements.

For more info, read the [accessibility overview](https://docs.microsoft.com/windows/uwp/accessibility/accessibility-overview) and [UWP Accessibility checklist](https://docs.microsoft.com/windows/uwp/accessibility/accessibility-checklist).

## Keyboard accessibility

- Tab order is logical and matches the visual layout. Reverse tabbing follows the same path. Focus can only be set to interactive controls (not static elements like labels).
- Arrowing is supported among child elements of a grouping or composite element (e.g., list). Grids can be fully navigated by arrow without requiring switching between left/right vs. up/down arrow keys.
- Keyboard activation of interactive controls can be accomplished. Enter key invokes default action. Spacebar activates the control which has focus.
- Keyboard shortcuts work & are documented.

## Accessible Names and Narrator

- Check if all interactive UI elements set the accessible name. ([More info](https://docs.microsoft.com/windows/uwp/accessibility/basic-accessibility-information))
- Narrator has been run to verify E2E programmatic access. ([See Narrator user guide](https://support.microsoft.com/en-us/help/22798/windows-10-narrator-get-started))
- Narrator: Launch of app results in correct default focus set which is automatically read out. Accessible name, role (control type) and value is read out for all controls.
- All elements (static text or interactive controls) can be reached with Narrator item navigation commands.
- Transitioning to a new UI results in default focus being set an appropriate & meaningful accessible name being read out.
- Invisible controls (e.g., used for layout purposes) are not reachable by Narrator. Dynamic content (e.g., alerts, validation errors, popups/windows appearing) are read out automatically.

## Visual experience

- Default contrast of 4.5:1 ([Color Contrast Analyser](https://www.paciellogroup.com/resources/contrastanalyser/) or other tools) is met or exceeded for all text using default/out-of-box settings.
- Display or text scaling causes UI to scale correctly and not result in controls being obscured.
- Ensure that your UI doesn’t use color as the only way to convey information.

## High contrast

- Switch to a high contrast theme and verify that the UI for your app is readable and usable.
- Both high contrast black & white themes are supported.
- Contrast of 14:1 is met or exceeded for all text ([Color Contrast Analyser](https://www.paciellogroup.com/resources/contrastanalyser/) or other tools ).
- Icons are visible.
- Colors match those defined by type in the Ease of Access high contrast preview.

## Windows Speech Recognition

- Controls can be invoked by saying their names.
- "Show numbers" works to visually disambiguate controls clearly and can be used to invoke controls.
- [Use speech recognition](https://support.microsoft.com/en-us/help/17208/windows-10-use-speech-recognition).

## Accessibility tools

- Use tools such as [Accessibility Insights](https://accessibilityinsights.io/) or [Inspect](https://msdn.microsoft.com/library/windows/desktop/Dd318521) to verify programmatic access, run diagnostic tools such as [AccChecker](https://msdn.microsoft.com/library/windows/desktop/Hh920985) to discover common errors, and verify the screen reading experience with Narrator. Other tools: [AccScope](https://msdn.microsoft.com/en-us/library/windows/desktop/dn433239.aspx) and [AccEvent](https://msdn.microsoft.com/en-us/library/windows/desktop/dd317979.aspx).

---

## Learn more

- [Getting started with the WinTS codebase](./getting-started-developers.md)
- [Understanding and authoring Templates](./templates.md)
- [Recording usage Telemetry](./telemetry.md)
- [All docs](./readme.md)
