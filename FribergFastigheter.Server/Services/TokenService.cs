using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Identity;
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
        /// The injected configuration.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Represents a symmetric security key.
        /// </summary>
        private readonly SymmetricSecurityKey _key;

        /// <summary>
        /// The injected user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">The injected configuration manager.</param>
        /// <param name="userManager">The injected user manager.</param>
        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
            _userManager = userManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a JWT token for a broker.
        /// </summary>
        /// <param name="broker">The broker to create the token for.</param>
        /// <returns>The created token as a <see cref="string"/>.</returns>
        public async Task<string> CreateToken(Broker broker)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, broker.User.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, broker.User.UserName!),
                new Claim(ApplicationUserClaims.BrokerId, broker.BrokerId.ToString()),
                new Claim(ApplicationUserClaims.BrokerFirmId, broker.BrokerFirm.BrokerFirmId.ToString()),
                new Claim(ApplicationUserClaims.UserId, broker.User.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(broker.User);

            foreach (string role in roles)
            {
                claims.Add(new Claim(ApplicationUserClaims.UserRole, role));
            }

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
