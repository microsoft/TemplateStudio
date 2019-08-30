# Table of Contents
* [Introduction](#introduction)
    * [Azure AD Authentication](#azure-ad-authentication--uwp-c-code-spec)
    * [Terminology](#terminology)
* [The Basics](#the-basics)
    * [Basic code flow](#basic-code-flow)
    * [App-specific variables](#app-specific-variables)
    * [Authentication endpoints](#authentication-endpoints)
* [The Code](#the-code)
    * [Project dependencies](#project-dependencies)
    * [App interface and required controls](#app-interface-and-required-controls)
    * [Creating an application instance](#creating-an-application-instance)
    * [Global variables](#global-variables)
    * [Authenticating the user](#authenticating-the-user)
    * [Implementing optional authentication](#implementing-optional-authentication)
    * [Logging in](#logging-in)
    * [Implementing logic after logging in](#implementing-logic-after-logging-in)
    * [Logging out](#logging-out)
    * [Implementing logic after logging out](#implementing-logic-after-logging-out)
    * [Handling exceptions](#handling-exceptions)
    * [Displaying exception message in-app](#displaying-exception-message-in-app)
* [The Graph Sample](#the-graph-sample)
    * [Implement GraphButton Click() function](#implement-graph-button-click-function)
    * [Call Graph API using HTTPS](#call-graph-api-using-https)
    * [Display Token Info](#display-token-info)

Introduction
============

Azure AD Authentication – UWP C\# Code Spec
-------------------------------------------

This document details the minimum generated C\# code required to implement Azure
AD (AAD) auth in a UWP C\# app.

This spec includes a couple of optional components – integrated authentication,
and an example function that queries Microsoft Graph and retrieves the logged-in
user’s basic profile information. This isn’t necessary for an app to implement
AAD authentication, but it’s useful to developers, in that it provides an easy
method of retrieving the logged-in user’s information (e.g. profile picture,
display name, email address, username).

There is also a small bit of XAML – a login button, and a text field that
outputs errors or the user info obtained via Graph.

For a working example of this spec, please clone and run the project located
here:

<https://github.com/ClairelyClaire/MSAL-Windows-Integrated-Auth-UWP-Sample/>

Terminology
-----------

In the context of authentication in general, there are a few concepts to keep in
mind:

| Term                    | Definition                                                                                                   |
|-------------------------|--------------------------------------------------------------------------------------------------------------|
| JWT                     | <ul><li>JavaScript Web Token</li><li>A digitally signed, period-delimited string of data</li><li>Must be cryptographically validated by receiving application (usually an API call via HTTPS)</li></ul>|
| Access Token            | <ul><li>The JWT returned by the auth provider, after the user has successfully authenticated</li><li>Passed to apps and services using HTTP Authorization header</li><li>Validated by the remote API, service, etc</li></ul>|
| Client ID               | <ul><li>GUID assigned when app is registered in the AAD portal</li><li>Required for AAD authentication</li></ul>|
| Client Secret           | <ul><li>A confidential key for server-side apps that act on behalf of a user or as a client independent of any user</li><li>Must be kept secure at all times</li></ul>|
| Integrated Auth         | <ul><li>Also called Windows Integrated Authentication (WIA) silent authentication, or pass-through authentication</li><li>Uses the logged-in user’s domain credentials (Kerberos ticket) to silently authenticate the user via AAD</li></ul>|
| Kerberos                | <ul><li>A network authentication standard and protocol used by Active Directory and AzureAD</li></ul>|
| Auth Endpoint           | <ul><li>The URI the app uses to prompt the user to log in</li></ul>|
| Microsoft Graph         | <ul><li>Microsoft’s web API surface for all things M365 and AzureAD</li><li>Not required for AAD authentication</li></ul>|
| Azure AD (AAD)          | <ul><li>Microsoft’s hosted Active Directory service</li><li>Required for AAD authentication</li></ul>|
| Microsoft Account (MSA) | <ul><li>Consumer accounts used across Microsoft’s products and services (Xbox Live, Outlook.com, etc.)</li><li>Formerly called Passport and Windows LiveID</li></ul>|
| MSAL                    | <ul><li>Microsoft Authentication Library</li><li>Microsoft-owned, open-source .NET library for wrapping OIDC implementation specifically for AAD and consumer Microsoft accounts</li></ul>|
| Azure B2B               | <ul><li>Business-to-business</li><li>B2B supports both consumer Microsoft accounts and AAD accounts from any tenant</li><li>Provides user policy and security controls identical to internal AAD</li></ul>|
| Azure B2C               | <ul><li>Business-to-consumer</li><li>B2C supports AAD, consumer Microsoft accounts, and external account providers (e.g. Google, Facebook)</li><li>Serves as a middleman or proxy between AAD and external account providers</li><li>Uses a separate, segregated directory from B2B and internal AAD</li></ul>|
| OIDC                    | <ul><li>OpenID Connect</li><li>An open standard used by various authentication providers, including AAD</li></ul>|
| OAuth2                  | <ul><li>The previous open authentication standard; deprecated and superseded by OIDC</li></ul>|
| OpenID                  | <ul><li>The original open auth standard; deprecated and superseded by OAuth2</li></ul>|

The Basics
==========

Basic Code Flow
---------------

This diagram illustrates the code flow and what should happen when the user
attempts to log in. The two optional components – pass-through authentication,
and retrieving user information from Graph – are included as well.

App-specific Variables
----------------------

There are a few variables that will be defined either by Template Studio, or
directly by the dev. These variable names are referenced in code in this spec.

| Variable       | Information                                                                        |
|----------------|------------------------------------------------------------------------------------|
| `ClientId`       | Programmatically obtained from AzureAD during initial template setup.              |
| `Endpoint`       | Auth endpoint for the app. This varies depending on app requirements.<br/>Base URL is always `https://login.microsoftonline.com/`.<br/>Append authentication endpoint from table below.|
| `AuthType`       | Options are integrated or interactive.                                             |
| `GraphSample`    | If the Graph sample code option is selected, include extra classes and references. |

Authentication Endpoints 
------------------------

|                  | <br/>&nbsp;&nbsp;&nbsp;AAD&nbsp;&nbsp;&nbsp; | <br/>&nbsp;&nbsp;&nbsp;MSA&nbsp;&nbsp;&nbsp; | Integrated<br/>Auth | <br/>Additional Info|
|-----------------:|:---:|:---:|:---------------:|:-----------------------:|
| `common/`        | ✔️ | ✔️  | ❌              |                       |
| `consumers/`     | ❌ | ✔️  | ❌              |                       | 
| `organizations/` | ✔️ | ❌  | ✔️              | *(all AAD accounts)*  |
| `domain.com/`      | ✔️ | ❌  | ✔️              | *(single AAD domain)* |

The Code
========

Project dependencies
--------------------

In order to implement this spec, one Nuget package is required –
[Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client),
aka MSAL. There are also a couple of system libraries that need to be included
for the Graph sample code.

```C#
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net.Http.Headers;
```

App interface and required controls
-----------------------------------

This spec includes some sample code to aid devs in understanding the data
returned by both Graph and any API or service. These sample bits should be
segregated from the main functional code, to make it easier for a dev to remove
the samples.

| Variable      | Description                                                                        |
|-------------:|:----------------------------------------------------------------------------------|
| `GraphButton` | Button – queries Graph when clicked.                                               |
| `UserButton`  | Button – logs the user in or out when clicked.                                     |
| `ResultText`  | Textblock – displays error message or successful response from MS Graph.           |
| `TokenText`   | Textblock – displays logged-in user’s name, and access token expiration and value. |

Suggested XAML for displaying these controls is below.

```XML
<StackPanel>
    <StackPanel Orientation="Horizontal"
        HorizontalAlignment="Right">
        <Button x:Name="GraphButton"
            Content="Call Graph"
            Click="GraphButton_Click"/>
        <Button x:Name="UserButton"
            Content="Log In"
            Click="UserButton_Click"/>
        <TextBlock x:Name="ErrorText"/>
    </StackPanel>
    <TextBlock Text="Response Body"/>
    <TextBox x:Name="ResultText"
        TextWrapping="Wrap"/>
    <TextBlock Text="Token Info"/>
    <TextBox x:Name="TokenText"
        TextWrapping="Wrap"/>
</StackPanel>
```

|---|


Creating an Application Instance
--------------------------------

The
[PublicClientApplication](https://docs.microsoft.com/dotnet/api/microsoft.identity.client.publicclientapplication?view=azure-dotnet)
MSAL object handles everything the app needs to direct the user to authenticate
via the Microsoft authentication provider.

This constructor takes a `ClientId` and an `Endpoint`. The `Endpoint` parameter is
required if using anything other than common. This should be invoked in the
application’s `App.xaml.cs` so that any page in the app can handle authentication
if necessary.

```C#
// value should be filled in during configuration (from app registration)
private static string ClientId = "b280a505-cc3d-4382-996e-50c3ea09af2d";

// prefix of endpoint does not change
private static string Endpoint = "https://login.microsoftonline.com/ ";

// append to Endpoint per user selection during configuration
Endpoint += "/organizations/";

public static PublicClientApplication PublicClientApp { get; } =
new PublicClientApplication(ClientId, Endpoint);
```

Also included in `App.xaml.cs` are two booleans, set during the dev’s
configuration of the template. These are referenced in code in this spec, and
are used to change application behavior based on dev requirements.

```C#
public bool IntegratedAuth = true;
public bool GraphSample = true;
```

Global Variables
----------------

The
[AuthenticationResult](https://docs.microsoft.com/dotnet/api/microsoft.identity.client.authenticationresult?view=azure-dotnet)
MSAL object handles the response from the Microsoft authentication provider,
whether that is an error message or a payload containing an app token. This
needs to be public or global, to facilitate app scenarios that have some
functionality behind authenticated access, with other functionality publicly
available. This spec puts this object in its own class named `Globals`, in a class
file named `Globals.cs`.

```C#
using Microsoft.Identity.Client;

namespace MSALSample
{
    public class Globals
    {
        public static AuthenticationResult Globals.AuthResult { get; set; } = null;
    }
}
```

Authenticating the User
-----------------------

Authentication follows the code flow described in this spec document. The sample
code below includes exception handling. Because there are a couple of factors
the dev can’t control – the user’s correct input of their own credentials, and
the responses from the authentication endpoint, exception handling is necessary
even in the most basic implementation.

This function should be referenced by the app based on how users will be
expected to login – silently at launch, clicking “login” somewhere in the UI,
etc. This spec assumes there is a clickable Login control in the app.

The functions for `LoginUser()` and `LogoutUser()` are detailed in this spec. The
function for the login button should be included in the code behind page
wherever the button is implemented. The easiest implementation is to keep the
control and its code in the application’s main page.

```C#
private void UserButton_Click(object sender, RoutedEventArgs e)
{
    Button cmd = sender as Button;

    if ((string)cmd.Content == "Log In")
        LoginUser();
    else
        LogoutUser();
}
```

Implementing Optional Authentication
------------------------------------

Referring back to the section titled “Creating an Application Instance”, the
`authResult` object stores the results of an authentication attempt, including the
access token needed to use other remote services and APIs. Implementing optional
authentication should rely on this object – you can either reference the result
object, or you can simply reference `LoginUser()` to ensure the user is properly
authenticated before accessing a particular page in a UWP app.

Logging In
----------

The
[GetAccountAsync](https://docs.microsoft.com/dotnet/api/microsoft.identity.client.clientapplicationbase.getaccountsasync?view=azure-dotnet)
MSAL method returns any valid access tokens in the application’s token cache.
This allows the app to log in silently, if a valid token is returned. Using
`FirstOrDefault` ensures null is returned if no tokens are available – without
this, an error will be thrown instead.

```C#
private async void LoginUser()
{
    // get auth tokens from app's cache
    IEnumerable<IAccount> accounts = await App.PublicClientApp.GetAccountsAsync();
    IAccount firstAccount = accounts.FirstOrDefault();

    // create empty list object to store exceptions
    List<Exception> exceptions = new List<Exception>();

    // handle authentication
    try
    {
        // if integrated auth is enabled, try that
        // otherwise, attempt auth with the first existing token from cache
        if (App.IntegratedAuth)
            authResult = await
                App.PublicClientApp.AcquireTokenByIntegratedWindowsAuthAsync(scopes);
        else
            authResult = await
                App.PublicClientApp.AcquireTokenSilentAsync(scopes, firstAccount);
    }
    // silent auth failed - catch exception
    catch (MsalException ex)
    {
        // try interactive auth
        try
        {
            authResult = await App.PublicClientApp.AcquireTokenAsync(scopes);
        }
        catch (MsalException msalex)
        {
            exceptions.Add(msalex);
        }
    }

    // if exceptions exist, pass the list to the handler
    if (exceptions.Count > 0)
    {
        ErrorHandler(exceptions);
    }
    // if an authResult exists, user is logged in
    if (authResult != null)
    {
        // TODO: Implement post-login logic
        PostLogin();
    }

}
```

<br>Implementing Logic After Logging In
---------------------------------------

Once the user is logged in, call the `PostLogin()` function to implement any logic
requiring authentication. At minimum, the “Log In” control’s text should be
updated to indicate the user is logged in.

In this spec, if the Graph sample code is enabled by the developer during
template configuration, this will also reveal a second button for calling the
Graph API.

```C#
private void PostLogin()
{
    if (App.GraphSample)
        GraphButton.Visibility = Visibility.Visible;
    
    // Update login button text
    UserButton.Content = "Log Out";
}
```

Logging Out
-----------

Logging out follows a much simpler flow than logging in – the logged-in user’s
account must be removed from the app’s token cache, and the `authResult` object is
nullified. This allows the developer to implement conditional logic checking the
user’s authentication status by validating whether or not `authResult` is null.

```C#
private async void LogoutUser()
{
    IEnumerable<IAccount> accounts = await App.PublicClientApp.GetAccountsAsync();
    IAccount firstAccount = accounts.FirstOrDefault();

    // create empty list object to store exceptions
    List<Exception> exceptions = new List<Exception>();

    try
    {
        await App.PublicClientApp.RemoveAsync(firstAccount);

        //nullify authResult - needed to ensure log out is complete
        Globals.AuthResult = null;

        // TODO: implement post-logout logic
        PostLogout();
    }
    catch (MsalException ex)
    {
        exceptions.Add(ex);

        ErrorHandler(exceptions);
    }
}
```

Implementing Logic After Logging Out
------------------------------------

Once the user is logged out, the UI should be updated accordingly. If the Graph
sample code is enabled, the Graph API button is hidden.

```C#
private void PostLogout()
{
    if (App.GraphSample)
        GraphButton.Visibility = Visibility.Collapsed;
    
    // Update login button text
    UserButton.Content = "Log In";            
}
```

Handling Exceptions
-------------------

Because authentication involves variables outside of the application or
developer’s control (user input and a remote auth endpoint), exception trapping
is a mandatory component of any auth implementation.

MSAL includes its own exception objects that inherit from `System.Exception`. They
are detailed
[here](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/exceptions).
The exception type provides clues as to what needs to be remedied, by either the
application or the user.

MSAL uses its own parent type,
[MsalException](https://docs.microsoft.com/dotnet/api/microsoft.identity.client.msalexception?view=azure-dotnet),
from which the other exception types inherit. There are three potential
exception types:

| Type                                                                                                                                       | Purpose                                                                                          |
|--------------------------------------------------------------------------------------------------------------------------------------------:|--------------------------------------------------------------------------------------------------|
| [MsalClientException](https://docs.microsoft.com/dotnet/api/microsoft.identity.client.msalclientexception?view=azure-dotnet)         | Thrown when the client is unable to log in, e.g. user cancels, network errors on client machine. |
| [MsalServiceException](https://docs.microsoft.com/dotnet/api/microsoft.identity.client.msalserviceexception?view=azure-dotnet)       | Thrown when AAD is unable to authenticate the user, e.g. wrong password, service outage.         |
| [MsalUiRequiredException](https://docs.microsoft.com/dotnet/api/microsoft.identity.client.msaluirequiredexception?view=azure-dotnet) | Thrown when silent or integrated authentication fails.                                           |

All errors include a `Messages` property, which contains a human-readable string
indicating the error. This is probably the most straightforward way to implement
basic error handling – the interface should display the error message in a
textblock near the login button in the application settings.
`MsalUiRequiredException` is the exception – these errors are meaningless to the
end-user; if silent authentication fails, the webview dialog immediately pops
and instructs the user to input their credentials.

In the `LoginUser()` and `LogoutUser()` functions described in this spec, all MSAL
exceptions are stored in a <code class="cs">List\<Exception\></code>` object that is then passed to an
error handler. The last error trapped is displayed in the app.

Additionally, if the sample code is enabled, errors are displayed in `ResultText`.

```C#
private void ErrorHandler (List<Exception> exceptions)
{
    // might be more than one error, so only display last message
    // MsalException.UnknownError can be omitted

    MsalException ex = exceptions[exceptions.Count - 1];

    string err = "";

    switch (ex.GetType()) {
        case MsalException:
            // do nothing - this is only ever an unknown error
            break;
        case MsalClientException:
            // grab this error - client failed
            err = ex.Message;
            break;
        case MsalServiceException:
            // grab this error - service failed
            err = ex.Message;
            break;
        case MsalUiRequiredException:
            // do nothing - silent auth failed
            break;
    }

    // if errs isn't empty, change the text of the texblock
    if (errs != "")
        DisplayResult(err,ErrorText);
    // if the sample is included, display the error(s)
    if (App.GraphSample)
        DisplayResult(string.Join("\n\n",(exceptions.OfType<MsalException>())),ResultText);
}

```

Displaying Exception Message In-App
-----------------------------------

The above sample references the `DisplayResult()` method, detailed below. This
method takes a string and the control that will display the string.

```C#
private void DisplayResult(string resultText, Control target)
{
    if (target is TextBox)
        ((TextBox)target).Text = resultText;
}
```

The Graph Sample
================

Implement GraphButton Click Function
------------------------------------

To reduce developer friction, this spec includes a small code sample that uses
the auth token to call the Graph API and retrieve the user’s basic profile. This
data is then displayed in the optional textboxes described in the spec. If the
Graph sample is enabled during template configuration, an extra button is
displayed that, when clicked, will call the API and return data.

```C#
private async void GraphButton_Click(object sender, RoutedEventArgs e)
{
    Button cmd = sender as Button;
    cmd.IsEnabled = false;
    cmd.Content = "Working...";

    ResultText.Text = string.Empty;
    TokenInfoText.Text = string.Empty;

    if (Globals.AuthResult != null)
    {
        ResultText.Text = await GetHttpContentWithToken(graphAPIEndpoint, authResult.AccessToken);
        DisplayBasicTokenInfo(authResult);
    }

    cmd.IsEnabled = true;
    cmd.Content = "Call Microsoft Graph API";
}
```

Call Graph API Using HTTPS
--------------------------

The Graph API uses HTTPS to receive requests from an application, and returns
the requested information to the application. In order to retreive data from the
API, the access token in `authResult` is passed in the authentication header of
the HTTPS request.

This function can be reused for other remote services, assuming the remote
service supports authentication via the HTTP authorization header. More details
on HTTP authorization are available
[here](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization).
Exception handling is mandatory – if the remote endpoint doesn’t respond or
responds with an HTTP error code (e.g. anything other than 200), an exception
will be thrown.

```C#
public async Task<string> GetHttpContentWithToken(string url, string token)
{
    HttpClient httpClient = new HttpClient();
    HttpResponseMessage response;
    try
    {
        // Create a new HTTP request object
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
        // Add the token in Authorization header
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        // Retreive the response via HTTP
        response = await httpClient.SendAsync(request);
        // Read the response body and return as a string
        string content = await response.Content.ReadAsStringAsync();
        return content;
    }
    catch (Exception ex)
    {
        // Return HTTP errors as a string
        return ex.ToString();
    }
}
```

Display Token Info
------------------

The other component of the sample code outputs information on the acquired
authentication token – the username, expiration date, and token itself (a JWT
string).

```C#
private void DisplayBasicTokenInfo(AuthenticationResult authResult)
{
    TokenInfoText.Text = "";
    if (Globals.AuthResult != null)
    {
        TokenInfoText.Text += $"User Name: {Globals.AuthResult.Account.Username}" + 
            Environment.NewLine;
        TokenInfoText.Text += $"Token Expires: {Globals.AuthResult.ExpiresOn.ToLocalTime()}" + 
            Environment.NewLine;
        TokenInfoText.Text += $"Access Token: {Globals.AuthResult.AccessToken}" + 
            Environment.NewLine;
    }
}
```
