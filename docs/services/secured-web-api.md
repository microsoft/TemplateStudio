# Secured Web API

This template will add a ASP.NET Core Web API that validates the JWToken passed by the UWP application.

The protection ensures that the API can only be called by:

* Applications that on behalf of the user request the right scopes.
* Users that have the right application roles.

## Configuration

After adding this service to your app you'll need to register a ClientId for the Uwp App and the Web API and update the configuration:

### Web API Configuration

1. Register Web API: Follow the steps from  https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app to register your Web API.
Update the appsettings.json file with the ClientId, TenantId and Audience.

2. Expose scopes on and roles: Follow the steps from https://docs.microsoft.com/azure/active-directory/develop/quickstart-configure-app-expose-web-apis to expose scopes and roles on your Web API.
Update the appsettings.json file with the configured Scope.

3. To assign users to your Web API follow the steps on https://docs.microsoft.com/azure/active-directory/develop/howto-restrict-your-app-to-a-set-of-users

### UWP Configuration

Register your own Client app following the steps on https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app to be able to access the Web API and configure the Client App to access the Web API following the steps from  https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis.

Update the App.config's IdentityClientId, ResourceId and WebApi Scope

## Understanding the code

This feature uses Forced or Optional Login from the Identity Service Group. [Learn more about using identity in WinTS generated apps](./identity.md).

### ServiceCollectionExtensions

The ServiceCollectionExtensions adds a AddJwtBearer Middleware to validate the token passed by the UWP App. It is configured to validate Token Issuer, Audience and LifeTime.

JwtBearer exposes two events:

- `OnAuthenticationFailed` which is invoked if there are errors during the authentication failed
- `OnTokenValidated`, which is invoked on successful authentication

### ItemController

The ItemController has two Authorize attributes that check the Claim and User Roles. For more information on claims and role-based authorization see

https://docs.microsoft.com/aspnet/core/security/authorization/claims?view=aspnetcore-2.2
https://docs.microsoft.com/aspnet/core/security/authorization/roles?view=aspnetcore-2.2

## Additional resources

- [Microsoft Identity Platform](https://docs.microsoft.com/azure/active-directory/develop/v2-overview)
- [Protecting Web API's](https://docs.microsoft.com/azure/active-directory/develop/scenario-protected-web-api-overview)