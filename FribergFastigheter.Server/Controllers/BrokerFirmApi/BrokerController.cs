using AutoMapper;
using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using FribergFastigheter.Shared.Dto.Broker;
using FribergFastigheter.Shared.Dto.Login;
using FribergFastigheter.Shared.Dto.Image;
using FribergFastigheter.Shared.Enums;
using FribergFastigheter.Shared.Services.AuthorizationHandlers.Broker.Data;
using FribergFastigheter.Server.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.BrokerFirmApi
{
    /// <summary>
    /// An API controller for the brokerfirm housings API.
    /// </summary>
    /// <!-- Author: Marcus, Jimmie -->
    /// <!-- Co Authors:  -->
    [Route("broker-firm-api")]
    [ApiController]
    public class BrokerController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected authorization service. 
        /// </summary>
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// The injected Auto Mapper.
        /// </summary>
        private readonly IMapper _autoMapper;

        /// <summary>
        /// The injected brokerfirm repository.
        /// </summary>
        private readonly IBrokerFirmRepository _brokerFirmRepository;

        /// <summary>
        /// The injected housing repository.
        /// </summary>
        private readonly IBrokerRepository _brokerRepository;

        /// <summary>
        /// The injected imageService.
        /// </summary>
        private readonly IImageService _imageService;

        /// <summary>
        /// The injected sign in manager.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// The injected token service. 
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// The injected user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="brokerRepository">The injected broker repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageService">The injected imageService properties.</param>
        /// <param name="brokerFirmRepository"The injected brokerfirm repository.></param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="tokenService">The injected token service. </param>
        /// <param name="userManager">The injected user manager.</param>
        /// <param name="authorizationService">The injected authorization service. </param>
        public BrokerController(IBrokerRepository brokerRepository, IMapper mapper, IImageService imageService, 
            IBrokerFirmRepository brokerFirmRepository, UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager, IAuthorizationService authorizationService)
        {
            _brokerRepository = brokerRepository;
            _autoMapper = mapper;
            _imageService = imageService;
            _brokerFirmRepository = brokerFirmRepository;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _authorizationService = authorizationService;
        }

        #endregion

        #region AdminApiEndpoints

        /// <summary>
        /// An API endpoint for editing broker objects. 
        /// </summary>
        /// <param name="id">The ID of the broker associated with the update</param>
        /// <param name="AdminEditBrokerDto">The serialized DTO object.</param>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors:  -->
        [Authorize(policy: ApplicationPolicies.BrokerAdmin)]
        [HttpPut("admin/broker/{id:int}")]
        public async Task<ActionResult> AdminEditBroker([Required] int id, [FromBody] AdminEditBrokerDto editBrokerDto)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);

            if (id != editBrokerDto.BrokerId)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.InputDataConflict, "The referenced broker doesn't match the supplied broker object."));
            }

            var broker = await _brokerRepository.GetBrokerByIdAsync(id);

            if (broker == null)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The referenced broker doesn't exists."));
            }

            var authData = new EditBrokerAuthorizationData(existingBrokerBrokerFirmId: brokerFirmId,
               existingBrokerBrokerId: id);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanEditBroker);

            if (result.Succeeded)
            {
                _autoMapper.Map(editBrokerDto, broker);
                await _brokerRepository.UpdateAsync(broker);
                return Ok(new MvcApiValueResponseDto<BrokerDto>(_autoMapper.Map<BrokerDto>(broker)));
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
            }   
        }


        #endregion

        #region ApiEndPoints

        /// <summary>
        /// API endpoint for registering a new broker.
        /// </summary>
        /// <param name="registerBrokerDto">The input data for the broker to create.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="IActionResult"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(Policy = ApplicationPolicies.BrokerAdmin)]
        [HttpPost("admin/brokers/create")]
        [ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiErrorResponseDto>(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateBroker([FromBody] RegisterBrokerDto registerBrokerDto)
        {
            try
            {
                var brokerId = int.Parse(User.FindFirst(x => x.Type == ApplicationUserClaims.BrokerId)!.Value);
                var brokerFirmId = int.Parse(User.FindFirst(x => x.Type == ApplicationUserClaims.BrokerFirmId)!.Value);
                var authData = new CreateBrokerAuthorizationData(brokerId);
                var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanCreateBroker);

                if (result.Succeeded)
                {
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
                            var brokerUserIdClaim = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId);
                            var user = await _userManager.FindByIdAsync(brokerUserIdClaim!.Value);

                            var brokerFirm = await _brokerFirmRepository.GetBrokerFirmByIdAsync(brokerFirmId);

                            var broker = new Broker(
                                brokerFirm!,
                                registerBrokerDto.Description,
                                user: applicationUser);
                            await _brokerRepository.AddAsync(broker);

                            return Ok(new MvcApiValueResponseDto<BrokerDto>(_autoMapper.Map<BrokerDto>(broker)));
                        }
                        else
                        {
                            return BadRequest(new MvcApiErrorResponseDto(roleResult));
                        }
                    }
                    else
                    {
                        return BadRequest(new MvcApiErrorResponseDto(createUserResult));
                    }
                }
                else
                {
                    return Unauthorized(new MvcApiErrorResponseDto(result));
                } 
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MvcApiErrorResponseDto(ApiErrorMessageTypes.GeneralError, "An unexpected error occurred."));
            }
        }

        /// <summary>
        /// An API endpoint for creating images. 
        /// </summary>
        /// <param name="id">The ID of the broker object the image belongs to</param>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpPost("broker/{id:int}/profile-image")]
        [ProducesResponseType<ImageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateProfileImage([Required] int id, [FromForm] IFormFile file)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var userRole = User.FindFirst(ApplicationUserClaims.UserRole)!.Value;

            if (!await _brokerFirmRepository.HaveBroker(brokerFirmId, id))
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceOwnershipConflict, "The referenced broker doesn't belong to the referenced broker firm object."));
            }
            else if (brokerId != id && userRole != ApplicationUserRoles.BrokerAdmin)
            {
                return Unauthorized(new MvcApiErrorResponseDto(ApiErrorMessageTypes.AuthorizationError, "Only administrators can modify other brokers."));
            };

            Image imageEntity = new(await _imageService.SaveImageToDiskAsync(file));
            await _brokerRepository.AddImage(id, imageEntity);
            var imageDto = _autoMapper.Map<ImageDto>(imageEntity);
            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, imageDto);

            return Ok(new MvcApiValueResponseDto<ImageDto>(imageDto));
        }

        /// <summary>
        /// An API endpoint for deleting broker objects. 
        /// </summary>
        /// <param name="id">The ID of the broker object to delete.</param>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors:  -->
        /// TODO: The delete a broker object does not work because of conflict with the housing object that have said broker as a property. Maybe a check and send a suitable response.
        [Authorize(policy: ApplicationPolicies.BrokerAdmin)]
        [HttpDelete("broker/{id}")]
        public async Task<ActionResult> DeleteBroker(int id)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);

            var authData = new DeleteBrokerAuthorizationData(existingBrokerBrokerFirmId: brokerFirmId);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanDeleteBroker);

            if (result.Succeeded)
            {
                var broker = await _brokerRepository.GetBrokerByIdAsync(id);

                if (broker!.ProfileImage != null)
                {
                    _imageService.DeleteImageFromDisk(broker.ProfileImage.FileName);
                    await _brokerRepository.DeleteProfileImage(id);
                }

                broker.IsDeleted = true;
                await _brokerRepository.UpdateAsync(broker);
                return Ok(new MvcApiEmptyResponseDto());
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
            }

            
        }

        /// <summary>
        /// An API endpoint for deleting housing images. 
        /// </summary>
        /// <param name="id">The ID of the broker object the image belongs to</param>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors:  -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpDelete("broker/{id:int}/profile-image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiErrorResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteProfileImage([Required] int id)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            var brokerId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerId)!.Value);
            var userRole = User.FindFirst(ApplicationUserClaims.UserRole)!.Value;

            if (!await _brokerFirmRepository.HaveBroker(brokerFirmId, id))
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceOwnershipConflict, "The referenced broker doesn't belong to the referenced broker firm object."));
            }
            else if (brokerId != id && userRole != ApplicationUserRoles.BrokerAdmin)
            {
                return Unauthorized(new MvcApiErrorResponseDto(ApiErrorMessageTypes.AuthorizationError, "Only administrators can modify other brokers."));
            }

            var image = await _brokerRepository.GetProfileImage(id);

            if (image != null)
            {
                await _brokerRepository.DeleteProfileImage(id);
                _imageService.DeleteImageFromDisk(image.FileName);
                return Ok(new MvcApiEmptyResponseDto());
            }

            // Should never get here
            return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "No image with that ID was found."));
        }

        /// <summary>
        /// An API endpoint for updating broker objects. 
        /// </summary>
        /// <param name="id">The ID of the broker associated with the update</param>
        /// <param name="editBrokerDto">The serialized DTO object.</param>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors:  -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpPut("broker/{id:int}")]
        public async Task<ActionResult> EditBroker([Required] int id, [FromBody] EditBrokerDto editBrokerDto)
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            
            if (id != editBrokerDto.BrokerId)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.InputDataConflict, "The referenced broker doesn't match the supplied broker object."));
            }

            var broker = await _brokerRepository.GetBrokerByIdAsync(id);

            if (broker == null)
            {
                return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, "The referenced broker doesn't exists."));
            }           

            var authData = new EditBrokerAuthorizationData(existingBrokerBrokerFirmId: brokerFirmId,
               existingBrokerBrokerId: id);
            var result = await _authorizationService.AuthorizeAsync(User, authData, ApplicationPolicies.CanEditBroker);

            if (result.Succeeded)
            {
                _autoMapper.Map(editBrokerDto, broker!);
                await _brokerRepository.UpdateAsync(broker!);
                return Ok(new MvcApiValueResponseDto<BrokerDto>(_autoMapper.Map<BrokerDto>(broker)));
            }
            else
            {
                return Unauthorized(new MvcApiErrorResponseDto(result));
            }
        }

        /// <summary>
        /// An API endpoint for fetching a broker object. 
        /// </summary>
        /// <param name="id">The ID of the broker to fetch.</param>
        /// <returns>An embedded collection of <see cref="BrokerDto"/>.</returns>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors:  -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("broker/{id:int}")]
        [ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrokerDto>>> GetBrokerById([Required] int id)
        {
            var broker = await _brokerRepository.GetBrokerByIdAsync(id);
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);

            if (broker == null)
            {
                return NotFound(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceNotFound, $"The broker with ID '{id}' was not found."));
            }
            else if (broker.BrokerFirm.BrokerFirmId != brokerFirmId)
            {
                return BadRequest(new MvcApiErrorResponseDto(ApiErrorMessageTypes.ResourceOwnershipConflict, "The referenced broker doesn't belong to the referenced broker firm."));
            }

            var result = _autoMapper.Map<BrokerDto>(broker);
            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, result);

            return Ok(new MvcApiValueResponseDto<BrokerDto>(result));
        }

        /// <summary>
        /// An API endpoint for searching broker objects by brokerFirmId. 
        /// </summary>
        /// <returns>An embedded collection of <see cref="BrokerDto"/>.</returns>
        /// <!-- Author: Marcus, Jimmie -->
        /// <!-- Co Authors:  -->
        [Authorize(policy: ApplicationPolicies.Broker)]
        [HttpGet("brokers")]
        [ProducesResponseType<BrokerDto>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrokerDto>>> GetBrokers()
        {
            var brokerFirmId = int.Parse(User.FindFirst(ApplicationUserClaims.BrokerFirmId)!.Value);
            
            var brokers = (await _brokerRepository.GetBrokersAsync(brokerFirmId: brokerFirmId))
                .Select(x => _autoMapper.Map<BrokerDto>(x))
                .ToList();

            _imageService.PrepareDto(HttpContext, BrokerFileController.ImageDownloadApiEndpoint, brokers);
            return Ok(new MvcApiValueResponseDto<List<BrokerDto>>(brokers));
        }

        /// <summary>
        /// API endpoint to handle broker login.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="IActionResult"/>.</returns>
        /// <!-- Author: Jimmie, Marcus -->
        /// <!-- Co Authors: -->       
        [HttpPost("brokers/login")]
        [ProducesResponseType<LoginResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiErrorResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginBroker([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.UserName);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

                    if (result != null)
                    {
                        var broker = await _brokerRepository.GetBrokerByUserIdAsync(user.Id);

                        if (broker != null)
                        {
                            return Ok(new MvcApiValueResponseDto<LoginResponseDto>(
                                new LoginResponseDto()
                            {
                                UserName = user!.UserName!,
                                Token = await _tokenService.CreateToken(broker)
                                }));
                        }
                    }
                }

                return Unauthorized(new ApiErrorResponseDto(HttpStatusCode.Unauthorized, new ApiErrorDto(ApiErrorMessageTypes.AuthorizationError, "User not found and/or password incorrect.")));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MvcApiErrorResponseDto(ApiErrorMessageTypes.GeneralError, "An unexpected error occurred."));
            }            
        }

        #endregion
    }
}

