namespace Shared;

public class ClientPrincipal
{
    public string IdentityProvider { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserDetails { get; set; } = string.Empty;
    public IEnumerable<string> UserRoles { get; set; } = [];
}

public class AuthResponse
{
    public ClientPrincipal? ClientPrincipal { get; set; }
}
