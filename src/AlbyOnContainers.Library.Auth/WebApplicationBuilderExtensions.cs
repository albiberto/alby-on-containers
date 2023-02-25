namespace AlbyOnContainers.Library.Auth;

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

public static class WebApplicationBuilderExtensions
{
    public static void AddKeyCloakAuthentication(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => false;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.Authority = configuration.GetValue<string>("Oidc:Authority");
                options.ClientId = configuration.GetValue<string>("Oidc:ClientId");
                options.ClientSecret = configuration.GetValue<string>("Oidc:ClientSecret");
                options.RequireHttpsMetadata = configuration.GetValue<bool>("Oidc:RequireHttpsMetadata"); // disable only in dev env
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = false;
                options.MapInboundClaims = true;
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("roles");

                options.Events = new()
                {
                    OnUserInformationReceived = context =>
                    {
                        MapKeyCloakRolesToRoleClaims(context);
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void AddKeyCloakAuthorization(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAuthorization();
    }

    private static void MapKeyCloakRolesToRoleClaims(UserInformationReceivedContext context)
    {
        if (context.Principal.Identity is not ClaimsIdentity claimsIdentity) return;

        if (context.User.RootElement.TryGetProperty("preferred_username", out var username)) claimsIdentity.AddClaim(new(ClaimTypes.Name, username.ToString()));

        if (context.User.RootElement.TryGetProperty("realm_access", out var realmAccess)
            && realmAccess.TryGetProperty("roles", out var globalRoles))
            foreach (var role in globalRoles.EnumerateArray())
                claimsIdentity.AddClaim(new(ClaimTypes.Role, role.ToString()));

        if (!context.User.RootElement.TryGetProperty("resource_access", out var clientAccess)
            || !clientAccess.TryGetProperty(context.Options.ClientId, out var client)
            || !client.TryGetProperty("roles", out var clientRoles)) return;

        foreach (var role in clientRoles.EnumerateArray()) claimsIdentity.AddClaim(new(ClaimTypes.Role, role.ToString()));
    }
}