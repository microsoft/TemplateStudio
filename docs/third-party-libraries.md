# 3rd party libraries updates and breaking changes

TS generated projects can include 3rd party libraries and references. These dependencies will inevitably be updated and changed over time.

The following points define how such changes will be handled.

* TS will aim to update to the latest versions of referenced libraries as quickly as possible.
* Where a referenced library introduces a breaking change, the templates will be adapted to incorporate such a change. Even if this means that projects built with an earlier version of the templates will fail when adding a page or feature with the new templates.
* Any changes to templates will be recorded in the release notes for the TS release that incorporates the changes.