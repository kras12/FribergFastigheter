using FribergFastigheter.Server.Data.DTO;
using FribergFastigheterApi.Data.Entities;

namespace FribergFastigheter.Server.Services
{
    public interface IImageService
    {
        void DeleteImageFromDisk(string fileName);
        Image SaveImageToDisk(ImageDto imageDto);
        void SetImageData(ImageDto image);
        void SetImageData(List<ImageDto> imageList);
    }
}