using Microsoft.AspNetCore.Hosting;

namespace She_He_Store.Helpers
{
    public static class ImageUploader
    {
        public static async Task<string>? UploadeImage(IWebHostEnvironment webhost, IFormFile file)
        {
            string wwwrootPath = webhost.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string path = Path.Combine(wwwrootPath + "/Images/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
