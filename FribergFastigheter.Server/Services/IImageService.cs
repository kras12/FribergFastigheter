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
        Task<string> SaveImageToDiskAsync(IFormFile imageFile);
        void PrepareDto(HttpContext httpContext, BrokerDto broker);
        void PrepareDto(HttpContext httpContext, List<BrokerDto> brokers);
        void PrepareDto(HttpContext httpContext, BrokerFirmDto brokerFirm);
        void PrepareDto(HttpContext httpContext, List<BrokerFirmDto> brokerFirms);
        void PrepareDto(HttpContext httpContext, HousingDto housing);
        void PrepareDto(HttpContext httpContext, List<HousingDto> housings); 
    }
}