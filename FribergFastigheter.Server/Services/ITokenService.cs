using FribergFastigheter.Server.Data.Entities;

namespace FribergFastigheter.Server.Services
{
	/// <summary>
	/// Interface for a class that creates JWT tokens for user classes.
	/// </summary>
	/// <!-- Author: Jimmie, Marcus -->
	/// <!-- Co Authors: -->
	public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT token for a broker.
        /// </summary>
        /// <param name="broker">The broker to create the token for.</param>
        /// <returns>The created token as a <see cref="string"/>.</returns>
        public Task<string> CreateToken(Broker broker);
    }
}
