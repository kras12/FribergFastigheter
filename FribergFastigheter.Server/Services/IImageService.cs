using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Shared.Enums;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;

namespace FribergFastigheter.Server.Services
{
    public interface IImageService
    {
        #region PrepareTransferMethods

        /// <summary>
        /// Sets necessary data to a DTO object to make it ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="image">The DTO object to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, ImageDto image);

        /// <summary>
        /// Sets necessary data to DTO objects to make them ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="images">The DTO objects to process.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, List<ImageDto> images);

        /// <summary>
        /// Sets necessary data to a DTO object to make it ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="brokerFirm">The DTO object to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, BrokerDto broker);

        /// <summary>
        /// Sets necessary data to DTO objects to make them ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="brokers">The DTO objects to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, List<BrokerDto> brokers);

        /// <summary>
        /// Sets necessary data to a DTO object to make it ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="brokerFirm">The DTO object to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, BrokerFirmSummaryDto brokerFirm);

        /// <summary>
        /// Sets necessary data to DTO objects to make them ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="brokerFirms">The DTO objects to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, List<BrokerFirmSummaryDto> brokerFirms);

        /// <summary>
        /// Sets necessary data to a DTO object to make it ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="brokerFirm">The DTO object to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, HousingDto housing);

        /// <summary>
        /// Sets necessary data to DTO objects to make them ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="housings">The DTO objects to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, List<HousingDto> housings);

        /// <summary>
        /// Sets necessary data to a DTO object to make it ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="brokerFirm">The DTO object to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, BrokerFirmDto brokerFirm);

        /// <summary>
        /// Sets necessary data to DTO objects to make them ready to be sent to the client. 
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageApiEndpoint">The API endpoint for fetching images in binary form.</param>
        /// <param name="brokerFirms">The DTO objects to process.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void PrepareDto(HttpContext httpContext, string imageApiEndpoint, List<BrokerFirmDto> brokerFirms);

        /// <summary>
        /// Gets an <see cref="ActionResult"/> derived object to support downloading of an image file.
        /// </summary>
        /// <param name="imageFileName">The file name of the image.</param>
        /// <returns>A <see cref="FileContentResult"/> if the file was found or null if not.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<FileContentResult> PrepareImageFileDownloadAsync(string imageFileName);

        /// <summary>
		/// Gets an <see cref="ActionResult"/> derived object to support downloading of an image file.
		/// </summary>
		/// <param name="imageFileNames">A collection of image file names.</param>
		/// <returns>A <see cref="FileStreamResult"/> if the files was found or null if not.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<FileStreamResult> PrepareImageFilesZipDownloadAsync(List<string> imageFileNames);

        #endregion

        #region DiskMethods

        /// <summary>
        /// Method for deleting images from disk.
        /// </summary>
        /// <param name="imageFileName">The image object to be deleted from disk.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        public void DeleteImageFromDisk(string imageFileName);

        /// <summary>
        /// Method for deleting images from disk.
        /// </summary>
        /// <param name="imageFileNames">The names of the images to delete.</param>
        /// /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public void DeleteImagesFromDisk(List<string> imageFileNames);

        /// <summary>
        /// Method for saving images to disk.
        /// </summary>
        /// <param name="imageFile">The file to save to the disk.</param>
        /// <returns>The file name of the saved file.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public Task<string> SaveImageToDiskAsync(IFormFile imageFile);

        #endregion
    }
}