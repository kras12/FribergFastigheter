using FribergFastigheter.Shared.Dto;
using FribergFastigheter.Shared.Enums;
using FribergFastigheterApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace FribergFastigheter.Server.Services
{
    public interface IImageService
    {
        void DeleteImageFromDisk(string fileName);
        Task<FileContentResult> PrepareImageFileDownloadAsync(string imageFileName);
        Task<FileStreamResult> PrepareImageFilesZipDownloadAsync(List<string> imageFileNames);
        Task<string> SaveImageToDiskAsync(string base64StringData, ImageTypes imageType);
        Task<string> SaveImageToDiskAsync(IFormFile imageFile);
        void SetImageData(HttpContext httpContext, ImageDto image, bool includeImageData = false);
        void SetImageData(HttpContext httpContext, List<ImageDto> imageList, bool includeImageData = false);
    }
}