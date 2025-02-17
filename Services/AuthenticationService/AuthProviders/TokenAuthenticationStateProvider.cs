﻿using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AuthProviders
{
    public class TokenAuthenticationStateProvider(Services.AuthenticationService.AuthenticationService.AuthProviders.TokenStorage tokenStorage) : AuthenticationStateProvider
    {
        public void StateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); // <- Does nothing
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            //var payload = jwt.Split('.')[1];
            //var jsonBytes = ParseBase64WithoutPadding(payload);
            //var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            //return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await tokenStorage.GetAccessToken();
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;

                case 3:
                    base64 += "=";
                    break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}