using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Shared.Enums;
using FribergFastigheterApi.Data.Entities;

namespace FribergFastigheter.Server.Services
{
    public interface IImageService
    {
        void DeleteImageFromDisk(string fileName);
        string SaveImageToDisk(string base64StringData, ImageTypes imageType);
		void SetImageData(ImageDto image);
        void SetImageData(List<ImageDto> imageList);
    }
}