using FribergFastigheter.Server.Data.DTO;
using FribergFastigheterApi.Data.Entities;
using Microsoft.Extensions.Configuration;
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
        /// <!-- Co Authors: -->
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

        #region Methods
        /// <summary>
        /// Method for converting images to Base64.
        /// </summary>
        /// <param name="image">The image object to be converted.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        public void SetImageData(ImageDto image)
        {
            byte[] imageArray = File.ReadAllBytes($"{_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value}/{image.FileName}");
            image.Base64 = Convert.ToBase64String(imageArray);
        }

        /// <summary>
        /// Method for converting a list of images to Base64.
        /// </summary>
        /// <param name="imageList">The List of image objects to be converted.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        public void SetImageData(List<ImageDto> imageList)
        {
            foreach (ImageDto image in imageList)
            {
                SetImageData(image);
            }
        }

        /// <summary>
        /// Method for saving images to disk.
        /// </summary>
        /// <param name="imageDto">The image object to be saved to disk.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        public Image SaveImageToDisk(ImageDto imageDto)
        {
            var filePath = Path.Combine(_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value, imageDto.FileName);
 
            var bytes = Convert.FromBase64String(imageDto.Base64);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
                return new Image(imageDto.FileName);
            }

        }

        /// <summary>
        /// Method for deleting images from disk.
        /// </summary>
        /// <param name="imageFileName">The image object to be deleted from disk.</param>
        /// /// <!-- Author: Marcus -->
        /// <!-- Co Authors: -->
        public void DeleteImageFromDisk(string imageFileName)
        {
            var filePath = Path.Combine(_configuration.GetSection("FileStorage").GetSection("UploadFolderPath").Value, imageFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        #endregion
    }
}
