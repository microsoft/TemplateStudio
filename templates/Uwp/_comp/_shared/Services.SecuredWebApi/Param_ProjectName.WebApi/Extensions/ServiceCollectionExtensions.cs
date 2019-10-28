using Param_RootNamespace.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    // Read more about Microsoft Identity Platform at https://docs.microsoft.com/azure/active-directory/develop/v2-overview
    // You can find detailed info on protecting Web API's on https://docs.microsoft.com/azure/active-directory/develop/scenario-protected-web-api-overview
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ProtectWebApiWithJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            // WTS TODO: Follow these steps to register your Web API and expose scopes and roles,
            // afterwards populate the appsettings.json with ClientId, Tenant, Audience and Scope
            // https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app
            // https://docs.microsoft.com/azure/active-directory/develop/quickstart-configure-app-expose-web-apis
            // To assign users to your web api: https://docs.microsoft.com/azure/active-directory/develop/howto-restrict-your-app-to-a-set-of-users
            var settings = new AuthenticationSettings();
            configuration.GetSection("AuthenticationSettings").Bind(settings);

            var tenantID = settings.TenantId;
            var audience = settings.Audience;
            var authority = $"https://login.windows.net/{tenantID}";

            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever());
            var openIdConfig = configurationManager.GetConfigurationAsync(CancellationToken.None).GetAwaiter().GetResult();

            // For multitenant scenarios and issuer validation please see
            // https://docs.microsoft.com/azure/active-directory/develop/howto-convert-app-to-be-multi-tenant#update-your-code-to-handle-multiple-issuer-values

            // You can get a list of issuers for the various Azure AD deployments (global & sovereign) from the following endpoint
            // https://login.microsoftonline.com/common/discovery/instance?authorization_endpoint=https://login.microsoftonline.com/common/oauth2/v2.0/authorize&api-version=1.1;
            var validissuers = new List<string>()
            {
                "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0",
                $"https://login.microsoftonline.com/{tenantID}/",
                $"https://login.microsoftonline.com/{tenantID}/v2.0",
                $"https://login.windows.net/{tenantID}/",
                $"https://login.microsoft.com/{tenantID}/",
                $"https://sts.windows.net/{tenantID}/"
            };

            var scope = settings.Scope;

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = authority;
                    options.Audience = audience;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuers = validissuers,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        IssuerSigningKeys = openIdConfig.SigningKeys,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // TODO WTS: This event is invoked if there where errors during token validation,
                            // please handle as appropriate to your scenario.
                            return Task.CompletedTask;
                        }
                    };
                });

            // Add Authorization with claim policy
            services.AddAuthorization(config =>
            {
                config.AddPolicy("SampleClaimPolicy", policy =>
                    policy
                        .RequireAuthenticatedUser()
                        .RequireClaim("http://schemas.microsoft.com/identity/claims/scope", scope));
            });

            return services;
        }
    }
}
