using FribergFastigheter.Server.Data.Constants;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FribergFastigheter.Server.Services
{
    /// <summary>
    /// Creates JWT tokens for user classes.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    public class TokenService : ITokenService
    {
        #region Fields

        /// <summary>
        /// The inject configuration.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Represents a symmetric security key.
        /// </summary>
        private readonly SymmetricSecurityKey _key;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">The inject configuration.</param>
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a JWT token for a broker.
        /// </summary>
        /// <param name="broker">The broker to create the token for.</param>
        /// <returns>The created token as a <see cref="string"/>.</returns>
        public string CreateToken(Broker broker)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, broker.User.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, broker.User.UserName!),
                new Claim(BrokerUserClaims.BrokerId, broker.BrokerId.ToString()),
                new Claim(BrokerUserClaims.BrokerFirmId, broker.BrokerFirm.BrokerFirmId.ToString()),
                new Claim(BrokerUserClaims.UserId, broker.User.Id.ToString())
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
