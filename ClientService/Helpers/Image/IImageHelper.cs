using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Helpers.Image
{
    public interface IImageHelper
    {
        Task<ImageUploadResult> UploadImage(string name, IFormFile pictureFile, string folderName = "upload");
    }
}
