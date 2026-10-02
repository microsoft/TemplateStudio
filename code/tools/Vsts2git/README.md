# Vsts2git build-log downloads

Configure `VsProjectUrl` in the Function application's deployment settings before
deploying this version. It must identify the trusted Azure DevOps organization
and project, for example:

```text
https://dev.azure.com/<organization>/<project>
https://<organization>.visualstudio.com/<project>
```

Use HTTPS on the default port, a DNS hostname, and an explicit project path.
Userinfo, query strings, fragments, IP literals, and loopback hosts are rejected.
This setting is deployment-controlled; never populate it from a webhook payload.
The existing `VsPAT` remains in protected application settings; its source is
unchanged. The empty `VsProjectUrl` entry in `settings.json` is a placeholder,
not an automatic configuration of a deployed Function App.

The function requires a positive integer `resource.id`. It constructs
`<VsProjectUrl>/_apis/build/builds/<id>/logs?api-version=7.1` itself, requests
`application/zip`, and ignores `resource.logs.url`. A missing or invalid
deployment setting fails closed before an authenticated request is sent.

The request follows the documented
[Azure DevOps Build Logs API](https://learn.microsoft.com/en-us/rest/api/azure/devops/build/builds/get-build-logs?view=azure-devops-rest-7.1).

Automatic redirects are disabled so the authenticated download cannot follow a
response to another destination. Only HTTP 200 responses are uploaded as logs.
Other HTTP statuses produce a warning and skip log upload; the build summary can
still be published without a log-download link.
