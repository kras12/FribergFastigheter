using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Shared.Enums;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Image = FribergFastigheterApi.Data.Entities.Image;

namespace FribergFastigheter.Server.Services
{
    public class ImageService : IImageService
    {
        /// <summary>
        /// A Service for handling Images.
        /// </summary>
        /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        ///
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructors

        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Properties

        private string UploadFolderPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetSection("FileStorage").GetSection("UploadFolderName").Value!);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for converting images to Base64.
        /// </summary>
        /// <param name="image">The image object to be converted.</param>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="embeddImageData">True to embedd the image file data.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        /// <param name="httpContext"></param>
        /// <param name="embeddImageData"></param>
        public void SetImageData(HttpContext httpContext, ImageDto image, bool embeddImageData = false)
        {
            byte[] imageArray = File.ReadAllBytes($"{UploadFolderPath}/{image.FileName}");
            image.ImageType = GetImageType(image.FileName);
            image.Url = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/Housing/Image/{image.FileName}";
            image.Base64 = embeddImageData ? Convert.ToBase64String(imageArray) : "";
        }

        /// <summary>
        /// Method for converting a list of images to Base64.
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <param name="imageList">The List of image objects to be converted.</param>
        /// <param name="embeddImageData">True to embedd the image file data.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        public void SetImageData(HttpContext httpContext, List<ImageDto> imageList, bool includeImageData = false)
        {
            foreach (ImageDto image in imageList)
            {
                SetImageData(httpContext, image, includeImageData);
            }
        }

		/// <summary>
		/// Method for saving images to disk.
		/// </summary>
		/// <param name="base64StringData">A base64 string representation of the file data.</param>
        /// <param name="imageType">The image type.</param>
        /// <returns>The file name of the saved file.</returns>
        /// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<string> SaveImageToDiskAsync(string base64StringData, ImageTypes imageType)
		{
            #region Checks           

            if (string.IsNullOrEmpty(base64StringData))
            {
				throw new ArgumentException($"The file data is empty.");
			}

            #endregion

            var filePath = RandomUniqueFilePath(imageType);
			await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(base64StringData));

            return Path.GetFileName(filePath);
		}

		/// <summary>
		/// Method for deleting images from disk.
		/// </summary>
		/// <param name="imageFileName">The image object to be deleted from disk.</param>
		/// /// <!-- Author: Marcus -->
		/// <!-- Co Authors: -->
		public void DeleteImageFromDisk(string imageFileName)
        {
            var filePath = Path.Combine(UploadFolderPath, imageFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

		/// <summary>
		/// Generates a unique and nonexisting file name.
		/// </summary>
		/// <param name="imageType">The image type.</param>
		/// <returns>The generated file name.</returns>
		/// <!-- Author: Marcus -->
		/// <!-- Co Authors: -->
		private string RandomUniqueFilePath(ImageTypes imageType)
        {
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string filePath;

            do
            {
                string newFileName = Enumerable.Repeat(chars, 32)
                    .Select(s => s[random.Next(s.Length)]).ToString()!;

                filePath = Path.Combine(UploadFolderPath, newFileName, GetImageFileExtension(imageType));
           
            }
            while (File.Exists(filePath)); 

            return filePath;
		}

        /// <summary>
        /// Gets the extension for the image type.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <returns><see cref="string"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private string GetImageFileExtension(ImageTypes imageType)
        {
            switch (imageType)
            {
                case ImageTypes.Jpeg:
                    return ".jpg";

				case ImageTypes.Png:
					return ".png";

				default:
                    throw new NotSupportedException($"The image type is not supported: {imageType}");
            }
        }

        /// <summary>
        /// Gets the image type from a file name.
        /// </summary>
        /// <param name="imageType">The file name.</param>
        /// <returns><see cref="ImageTypes"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private ImageTypes GetImageType(string fileName)
		{
			switch (Path.GetExtension(fileName.ToLower()))
			{
                case ".jpg":
                    return ImageTypes.Jpeg;

				case ".png":
					return ImageTypes.Png;

				default:
					throw new NotSupportedException($"The file extension is not supported: {Path.GetExtension(fileName)}");
			}
		}

        /// <summary>
		/// Gets the extension for the image type.
		/// </summary>
		/// <param name="imageType">The image type.</param>
		/// <returns></returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
		private string GetImageContentType(ImageTypes imageType)
        {
            switch (imageType)
            {
                case ImageTypes.Jpeg:
                    return "application/jpg";

                case ImageTypes.Png:
                    return "application/png";

                default:
                    throw new NotSupportedException($"The image type is not supported: {imageType}");
            }
        }

        /// <summary>
		/// Gets an <see cref="ActionResult"/> derived object to support downloading of an image file.
		/// </summary>
		/// <param name="imageType">The image type.</param>
		/// <returns>A <see cref="FileContentResult"/> if the file was found or null if not.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<FileContentResult?> PrepareImageFileDownloadAsync(string imageFileName)
        {
            var filePath = Path.Combine(UploadFolderPath, imageFileName);

            if (File.Exists(filePath))
            {
               return new FileContentResult(await File.ReadAllBytesAsync(filePath), GetImageContentType(GetImageType(imageFileName)))
                {
                    FileDownloadName = imageFileName
                };
            }

            return null;
        }

        #endregion
    }
}
