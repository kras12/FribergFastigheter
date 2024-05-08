using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FribergFastigheter.Client.Services
{
    /// <summary>
    /// A service that provides information about the authentication state of the current user.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        #region Fields

        /// <summary>
        /// The injected local storage service.
        /// </summary>
        private readonly ILocalStorageService _localStorageService;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="localStorageService">The injected local storage service.</param>
        public ApiAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an authentication state that describes the user. 
        /// </summary>
        /// <returns>A <see cref="Task"/> that contains the <see cref="AuthenticationState"/>.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorageService.GetItemAsStringAsync("FribergFastigheterApiToken");

            if (string.IsNullOrEmpty(savedToken))
            {                
                return new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity()));
            }

            var parsedToken = new JwtSecurityTokenHandler().ReadJwtToken(savedToken);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(parsedToken.Claims, "BrokerFirmApiUser")));
        }

        /// <summary>
        /// Gets the token from local storage. 
        /// </summary>
        /// <returns>A <see cref="Task"/> that contains token as a <see cref="string"/>.</returns>
        public async Task<string?> GetTokenAsync()
        {
            var token = await _localStorageService.GetItemAsStringAsync("FribergFastigheterApiToken");

            if (token != null)
            {
                var expiration = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;

                if (expiration > DateTime.Now)
                {
                    return token;
                }
                else
                {
                    await _localStorageService.RemoveItemAsync("FribergFastigheterApiToken");
                }
            }

            return null;
        }

        /// <summary>
        /// Removes the token from local storage. 
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        public async Task RemoveTokenAsync()
        {
            await _localStorageService.RemoveItemAsync("FribergFastigheterApiToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Stores the token in local storage.
        /// </summary>
        /// <param name="token">The token to store.</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        public async Task SetTokenAsync(string token)
        {
            await _localStorageService.SetItemAsStringAsync("FribergFastigheterApiToken", token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        #endregion
    }
}
