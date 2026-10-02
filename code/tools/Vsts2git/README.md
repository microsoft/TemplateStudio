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
Keep `VsPAT` in protected application settings with only the build-log read
permissions required for that project. Do not put real credentials in
`settings.json` or source control.

The function requires a positive integer `resource.id`. It constructs
`<VsProjectUrl>/_apis/build/builds/<id>/logs?api-version=7.1` itself, requests
`application/zip`, and ignores
`resource.logs.url`, including when that field is absent. A missing or invalid
deployment setting fails closed before an authenticated request is sent.

The request follows the documented
[Azure DevOps Build Logs API](https://learn.microsoft.com/en-us/rest/api/azure/devops/build/builds/get-build-logs?view=azure-devops-rest-7.1).

Automatic redirects are disabled. Redirect/error responses fail rather than
being published as a successful download. Responses are buffered with a
**100 MiB (104,857,600-byte)** limit, including responses without a declared
content length. The download timeout is **12,000 seconds** and includes receiving
the response body. The Function host or hosting plan may impose a shorter
execution timeout; this client setting does not override host limits.
No blob is opened until the download completes successfully within these limits.

Before deploying, confirm that the function is still needed, restrict access to
its Function key, review outbound and blob-access policies, and review the PAT's
scope and exposure history. Rotate the PAT and Function key if exposure is
suspected. A source change alone does not deploy the fix or establish that a
previous deployment was uncompromised.

Build this legacy Function project with Visual Studio's full-framework MSBuild.
Its Functions metadata generator is not compatible with `dotnet build`.
