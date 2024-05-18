using FribergFastigheter.Shared.Dto.Api;
using FribergFastigheter.Shared.Dto.Login;

namespace FribergFastigheter.Client.Services.Authentication
{
    /// <summary>
    /// Authentication service interface for brokers.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    public interface IBrokerAuthenticationService
    {
        #region Methods

        /// <summary>
        /// Attempts to login a broker.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="ApiResponseDto{T}"/> with the result.</returns>
        public Task<ApiResponseDto<LoginResponseDto>> Login(LoginDto loginDto);

        /// <summary>
        /// Logs out the broker.
        /// </summary>
        /// <returns></returns>
        public Task Logout();

        #endregion
    }
}
