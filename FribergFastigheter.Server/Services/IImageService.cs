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
        Task<FileContentResult?> PrepareImageFileDownloadAsync(string imageFileName);
        string SaveImageToDisk(string base64StringData, ImageTypes imageType);
		void SetImageData(HttpContext httpContext, ImageDto image, bool includeImageData = false);
        void SetImageData(HttpContext httpContext, List<ImageDto> imageList, bool includeImageData = false);
    }
}