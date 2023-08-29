using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider {
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage) {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        try {
            var userSessionStorage = await _sessionStorage.GetAsync<UserSession>("UserSession");
            UserSession session = userSessionStorage.Success ? userSessionStorage.Value : null;

            if (session == null) {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }

            ClaimsPrincipal claims = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>{
                new Claim(ClaimTypes.Name, session.UserName),
                new Claim(ClaimTypes.Role, session.Role)
            }, "CustomAuth"));
            return await Task.FromResult(new AuthenticationState(claims));
        } catch {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationState(UserSession session) {
        ClaimsPrincipal claims;

        if (session != null) {
            await _sessionStorage.SetAsync("UserSession", session);
            claims = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>{
                new Claim(ClaimTypes.Name, session.UserName),
                new Claim(ClaimTypes.Role, session.Role)
            }));
        } else {
            await _sessionStorage.DeleteAsync("UserSession");
            claims = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
    }
}