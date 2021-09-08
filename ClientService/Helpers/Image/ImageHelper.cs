using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClientService.Helpers.Image
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string imgFolder = "img";
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = env.WebRootPath;
        }
        public async Task<ImageUploadResult> UploadImage(string name, IFormFile pictureFile, string folderName = "upload")
        {
            if (pictureFile==null)
            {
                return new ImageUploadResult { Success = false };
            }

            string fileExtension = Path.GetExtension(pictureFile.FileName);

            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                return new ImageUploadResult { Success = false };
            }

            if (!Directory.Exists($"{_wwwroot}/{imgFolder}/{folderName}"))
            {
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}/{folderName}");
            }

            string fileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            string fullFileName = $"{name}_{Guid.NewGuid()}_{dateTime.Year}_{dateTime.Month}_{dateTime.Day}_{fileName}{fileExtension}";
            var path = Path.Combine($"{_wwwroot}/{imgFolder}/{folderName}/", $"{fullFileName}");

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            return new ImageUploadResult
            {
                Success = true,
                FullName = $"{folderName}/{fullFileName}",
                Extension = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = pictureFile.Length
            };
        }
    }
}
