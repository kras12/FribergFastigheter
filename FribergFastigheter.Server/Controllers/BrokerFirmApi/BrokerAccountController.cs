using AutoMapper;
using FribergFastigheter.Server.Data.Constants;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using FribergFastigheter.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace FribergFastigheter.Server.Controllers.BrokerFirmApi
{
    /// <summary>
    /// An API controller to handle broker registration and login.
    /// </summary>
    /// <!-- Author: Jimmie, Marcus -->
    /// <!-- Co Authors: -->
    [Route("broker-firm-api")]
    [ApiController]
    public class BrokerAccountController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// The injected token service. 
        /// </summary>
        private readonly ITokenService _tokenService;

        // <summary>
        /// The injected signin manager.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        // <summary>
        /// The injected broker firm repository.
        /// </summary>
        private readonly IBrokerFirmRepository _brokerFirmRepository;

        // <summary>
        /// The injected broker repository
        /// </summary>
        private readonly IBrokerRepository _brokerRepository;

        // <summary>
        /// The injected auto mapper.
        /// </summary>
        private readonly IMapper _autoMapper;

        #endregion

        #region Constructors

        public BrokerAccountController(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IBrokerFirmRepository brokerFirmRepository,
            IBrokerRepository brokerRepository,
            IMapper autoMapper,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _brokerFirmRepository = brokerFirmRepository;
            _brokerRepository = brokerRepository;
            _autoMapper = autoMapper;
            _signInManager = signInManager;
        }

        #endregion

        #region Methods

        // TODO - Only allow BrokerAdmins
        /// <summary>
        /// API endpoint for registering a new broker.
        /// </summary>
        /// <param name="registerBrokerDto">The input data for the broker to create.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="IActionResult"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(Policy = ApplicationPolicies.BrokerAdmin)]
        [HttpPost("broker/register")]
        [ProducesResponseType<CreatedBrokerDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
      
        public async Task<IActionResult> Register([FromBody] RegisterBrokerDto registerBrokerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "Model validation failed"));
                }

                var applicationUser = new ApplicationUser(
                    registerBrokerDto.FirstName,
                    registerBrokerDto.LastName,
                    registerBrokerDto.Email,
                    registerBrokerDto.Email,
                    registerBrokerDto.PhoneNumber,
                    registerBrokerDto.Password
                    );
                
                var createUserResult = await _userManager.CreateAsync(applicationUser, registerBrokerDto.Password);

                if (createUserResult.Succeeded) 
                {
                    var roleResult = await _userManager.AddToRoleAsync(applicationUser, ApplicationUserRoles.Broker);

                    if (roleResult.Succeeded)
                    {
                        var brokerUserClaim = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId);
                        var user = _userManager.Users.First(x => x.Id == brokerUserClaim!.Value);

                        var brokerFirmClaim = User.FindFirst(x => x.Type == ApplicationUserClaims.BrokerFirmId);
                        var brokerFirm = await _brokerFirmRepository.GetBrokerFirmByIdAsync(int.Parse(brokerFirmClaim!.Value));

                        var broker = new Broker(
                            brokerFirm!,
                            registerBrokerDto.Description,
                            user: applicationUser);
                        await _brokerRepository.AddAsync(broker);
 
                        return Ok(_autoMapper.Map<CreatedBrokerDto>(broker.User));
                    }
                    else
                    {
                        return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "Failed to add user role."));
                    }
                }
                else
                {
                    return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "Failed to create user."));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "User registration failed."));
            }
        }

        /// <summary>
        /// API endpoint to handle broker login.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="IActionResult"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->       
        [HttpPost("broker/login")]
        [ProducesResponseType<LoginResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessageDto(System.Net.HttpStatusCode.BadRequest, "Model validation failed"));
            }

            var user = _userManager.Users.First(x => x.UserName == loginDto.UserName);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

                if (result != null)
                {
                    var broker = await _brokerRepository.GetBrokerByUserIdAsync(user.Id);

                    if (broker != null)
                    {
                        return Ok(new LoginResponseDto()
                        {
                            UserName = user!.UserName!,
                            Token = await _tokenService.CreateToken(broker)
                        });
                    }                    
                }                
            }

            return Unauthorized(new ErrorMessageDto(System.Net.HttpStatusCode.Unauthorized, "User not found and/or password incorrect."));
        }

        #endregion
    }
}
