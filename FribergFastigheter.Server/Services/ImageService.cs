using FribergFastigheter.Data.Entities;
using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Shared.Enums;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: Jimmie -->
        /// <param name="httpContext"></param>
        /// <param name="embeddImageData"></param>
        public void SetImageData(HttpContext httpContext, ImageDto image)
        {
            image.ImageType = GetImageType(image.FileName);
            image.Url = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/Housing/Image/{image.FileName}";
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
                SetImageData(httpContext, image);
            }
        }

        /// <summary>
		/// Method for saving images to disk.
		/// </summary>
		/// <param name="imageFile">The file to save to the disk.</param>
        /// <returns>The file name of the saved file.</returns>
        /// <!-- Author: Jimmie -->
		/// <!-- Co Authors: -->
		public async Task<string> SaveImageToDiskAsync(IFormFile imageFile)
        {
            #region Checks           

            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile), $"The value of parameter '{imageFile}' can't be null.");
            }

            #endregion

            string filePath = RandomUniqueFilePath(GetImageType(imageFile.FileName));
            
            using (var sourceStream = imageFile.OpenReadStream())
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await sourceStream.CopyToAsync(fileStream);
                }
            }

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
		/// <!-- Co Authors: Jimmmie -->
		private string RandomUniqueFilePath(ImageTypes imageType)
        {
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string filePath;

            do
            {
                string randomName = new(Enumerable.Repeat(chars, 32)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                filePath = Path.Combine(UploadFolderPath, CreateImageFileName(imageType, randomName));
           
            }
            while (File.Exists(filePath)); 

            return filePath;
		}

        /// <summary>
        /// Creates a file name by combining a name with the proper file extension.
        /// </summary>
        /// <param name="imageType">The type of the image.</param>
        /// <param name="fileNameWithoutExtension">The wanted filename without the extension.</param>
        /// <returns>A <see cref="string"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        private string CreateImageFileName(ImageTypes imageType, string fileNameWithoutExtension)
        {
            StringBuilder stringBuilder = new (fileNameWithoutExtension);

            switch (imageType)
            {
                case ImageTypes.Jpeg:
                    stringBuilder.Append(".jpg");
                    break;

				case ImageTypes.Png:
                    stringBuilder.Append(".png");
                    break;

                default:
                    throw new NotSupportedException($"The image type is not supported: {imageType}");
            }

            return stringBuilder.ToString();
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
		/// <param name="imageFileName">The file name of the image.</param>
		/// <returns>A <see cref="FileContentResult"/> if the file was found or null if not.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<FileContentResult> PrepareImageFileDownloadAsync(string imageFileName)
        {
            var filePath = Path.Combine(UploadFolderPath, imageFileName);

            if (!File.Exists(filePath))
            {
                throw new IOException($"The file '{imageFileName}' doesn't exists");
            }

            return new FileContentResult(await File.ReadAllBytesAsync(filePath), GetImageContentType(GetImageType(imageFileName)))
            {
                FileDownloadName = imageFileName
            };
        }

        /// <summary>
		/// Gets an <see cref="ActionResult"/> derived object to support downloading of an image file.
		/// </summary>
		/// <param name="imageFileNames">A collection of image file names.</param>
		/// <returns>A <see cref="FileStreamResult"/> if the files was found or null if not.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public async Task<FileStreamResult> PrepareImageFilesZipDownloadAsync(List<string> imageFileNames)
        {
            if (imageFileNames.Count == 0)
            {
                throw new ArgumentException($"The collection '{imageFileNames}' can't be empty.", nameof(imageFileNames));
            }

            MemoryStream memoryStream = new();

            using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (var imageFileName in imageFileNames)
                {
                    var filePath = Path.Combine(UploadFolderPath, imageFileName);

                    if (!File.Exists(filePath))
                    {
                        throw new IOException($"The file '{imageFileName}' doesn't exists");
                    }
                    var entry = archive.CreateEntry(imageFileName);

                    using (var entryStream = entry.Open())
                    {
                        using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }                
            }

            memoryStream.Position = 0;
            return new FileStreamResult(memoryStream, "application/zip")
            {
                FileDownloadName = $"images-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss_fff")}-{new Random().Next(100_000, 1_000_000)}.zip"
            };
        }

        #endregion
    }
}
