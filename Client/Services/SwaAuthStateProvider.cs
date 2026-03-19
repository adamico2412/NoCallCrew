using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Shared;

namespace Client.Services;

public class SwaAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _http;

    public SwaAuthStateProvider(HttpClient http)
    {
        _http = http;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _http.GetAsync("/.auth/me");

            if (!response.IsSuccessStatusCode)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            var principal = authResponse?.ClientPrincipal;

            if (principal is null || string.IsNullOrEmpty(principal.UserDetails))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, principal.UserDetails),
                new(ClaimTypes.NameIdentifier, principal.UserId),
                new("identity_provider", principal.IdentityProvider)
            };

            foreach (var role in principal.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "staticwebauth");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}
