using AutoMapper;
using FribergFastigheter.Server.Data.Interfaces;
using FribergFastigheter.Server.Services;
using FribergFastigheterApi.Data.DatabaseContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using FribergFastigheter.Shared.Dto.Error;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FribergFastigheter.Server.Controllers.HousingApi
{
    /// <summary>
    /// An API controller handling file downloads.
    /// </summary>
    /// <!-- Author: Jimmie -->
    /// <!-- Co Authors: -->
    [Route(BaseApiEndPoint)]
    [ApiController]
    public class HousingFileController : ControllerBase
    {
        #region Constants

        /// <summary>
        /// The base API endpoint
        /// </summary>
        private const string BaseApiEndPoint = "housing-api";
                
        /// <summary>
        /// The API endpoint for image downloads. 
        /// </summary>
        public const string ImageDownloadApiEndpoint = $"{BaseApiEndPoint}/{ImageDownloadApiEndpointSegment}";

        /// <summary>
        /// The API endpoint segment for the method handling the image downloads.
        /// </summary>
        private const string ImageDownloadApiEndpointSegment = "image";

        #endregion

        #region Fields

        /// <summary>
        /// The injected imageService properties.
        /// </summary>
        private readonly IImageService _imageService;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="imageService">The injected imageService properties.</param>
        public HousingFileController( IImageService imageService)
        {

            _imageService = imageService;
        }

        #endregion

        #region ApiEndPoints

        /// <summary>
        /// An API endpoint for retrieving an image file. 
        /// </summary>
        /// <param name="fileName">The file name of the image to fetch.</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        [HttpGet("image/{fileName}")]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiErrorResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageFile(string fileName)
        {
            var fileResult = await _imageService.PrepareImageFileDownloadAsync(fileName);

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
