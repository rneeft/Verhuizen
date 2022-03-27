using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Verhuizen;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorage;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await localStorage.GetItemAsStringAsync("key");

        ClaimsIdentity identity;

        var claims = new List<Claim>();
        if (!string.IsNullOrWhiteSpace(savedToken))
        {
            identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("Key", savedToken)
            }, "Token");
        }
        else
        {
            identity = new ClaimsIdentity();
        }
        
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));
        
        return state;
    }
}
