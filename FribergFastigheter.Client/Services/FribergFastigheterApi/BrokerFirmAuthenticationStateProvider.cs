using Blazored.LocalStorage;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FribergFastigheter.Client.Services.FribergFastigheterApi
{
    /// <summary>
    /// A service that provides information about the authentication state of the current user.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class BrokerFirmAuthenticationStateProvider : AuthenticationStateProvider
    {
        #region Constants

        /// <summary>
        /// The storage key for the JWT token.
        /// </summary>
        private const string JwtTokenStorageKey = "FribergFastigheterApiToken";

        #endregion

        #region Fields

        /// <summary>
        /// The injected local storage service.
        /// </summary>
        private readonly ILocalStorageService _localStorageService;

        /// <summary>
        /// The token. 
        /// </summary>
        private string? _token = null;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="localStorageService">The injected local storage service.</param>
        public BrokerFirmAuthenticationStateProvider(ILocalStorageService localStorageService)
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
            if (_token == null)
            {
                _token = await GetTokenAsync();
            }

            if (string.IsNullOrEmpty(_token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var parsedToken = new JwtSecurityTokenHandler().ReadJwtToken(_token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                parsedToken.Claims, authenticationType: "BrokerFirmApiUser", 
                nameType: JwtRegisteredClaimNames.GivenName, 
                roleType: ApplicationUserClaims.UserRole)));
        }        

        /// <summary>
        /// Gets the token from local storage. 
        /// </summary>
        /// <returns>A <see cref="Task"/> that contains token as a <see cref="string"/>.</returns>
        public async Task<string?> GetTokenAsync()
        {
            if (_token == null)
            {
                _token = await _localStorageService.GetItemAsStringAsync(JwtTokenStorageKey);
            }

            if (_token != null)
            {
                var expiration = new JwtSecurityTokenHandler().ReadJwtToken(_token).ValidTo;

                if (expiration > DateTime.Now)
                {
                    return _token;
                }
                else
                {
                    _token = null;
                    await _localStorageService.RemoveItemAsync(JwtTokenStorageKey);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
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
            await _localStorageService.RemoveItemAsync(JwtTokenStorageKey);
            _token = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Stores the token in local storage.
        /// </summary>
        /// <param name="token">The token to store.</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        public async Task SetTokenAsync(string token)
        {
            await _localStorageService.SetItemAsStringAsync(JwtTokenStorageKey, token);
            _token = token;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        #endregion
    }
}
