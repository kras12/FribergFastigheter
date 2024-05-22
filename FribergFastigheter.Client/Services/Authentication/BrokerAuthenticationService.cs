using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Dto.Api;
using FribergFastigheter.Shared.Dto.Login;
using Microsoft.AspNetCore.Components.Authorization;

namespace FribergFastigheter.Client.Services.Authentication
{
    /// <summary>
    /// Authentication service for brokers.
    /// </summary>
    /// <!-- Author: Marcus -->
    /// <!-- Co Authors: -->
    public class BrokerAuthenticationService : IBrokerAuthenticationService
    {
        #region Fields

        /// <summary>
        /// The injected authentication state provider.
        /// </summary>
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        /// <summary>
        /// The injected broker firm API service.
        /// </summary>
        private readonly IBrokerFirmApiService _brokerFirmApiService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authenticationStateProvider">The injected authentication state provider.</param>
        /// <param name="brokerFirmApiService">The injected broker firm API service.</param>
        public BrokerAuthenticationService(AuthenticationStateProvider authenticationStateProvider, IBrokerFirmApiService brokerFirmApiService)
        {
            _brokerFirmApiService = brokerFirmApiService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to login a broker.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="ApiResponseDto{T}"/> with the result.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<ApiResponseDto<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var response = await _brokerFirmApiService.Login(loginDto);

            if (response.Success)
            {
                await((BrokerFirmAuthenticationStateProvider)_authenticationStateProvider).SetTokenAsync(response.Value!.Token);
            }

            return response;
        }

        /// <summary>
        /// Logs out the broker.
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            await((BrokerFirmAuthenticationStateProvider)_authenticationStateProvider).RemoveTokenAsync();
        }

        #endregion
    }
}
