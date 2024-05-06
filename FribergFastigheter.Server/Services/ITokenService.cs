using FribergFastigheter.Server.Data.Entities;

namespace FribergFastigheter.Server.Services
{
    /// <summary>
    /// Interface for a class that creates JWT tokens for user classes.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT token for a broker.
        /// </summary>
        /// <param name="broker">The broker to create the token for.</param>
        /// <returns>The created token as a <see cref="string"/>.</returns>
        public string CreateToken(Broker broker);
    }
}
