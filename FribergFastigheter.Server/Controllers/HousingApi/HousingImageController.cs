using AutoMapper;
using FribergFastigheter.Data.Entities;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.HousingApi
{
    /// <summary>
    /// An API controller for the housing image API.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    [Route("api/Housing/Image")]
    [ApiController]
    public class HousingImageController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected imageService properties.
        /// </summary>
        private readonly IImageService _imageService;
        #endregion

        #region Constructors

        public HousingImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

		#endregion

		#region ApiEndPoints		

        /// <summary>
        /// An API endpoint for retrieving a housing image file. 
        /// </summary>
        /// <param name="imageFileName">The file name of the image to fetch.</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet("{imageFileName}")]
        [ProducesResponseType<HousingDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorMessageDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHousingImageFile(string imageFileName)
        {
            var fileResult = await _imageService.PrepareImageFileDownloadAsync(imageFileName);

            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return NotFound();
            }
        }

        #endregion
    }
}
