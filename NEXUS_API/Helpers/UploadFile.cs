using System.Reflection.Metadata;

namespace NEXUS_API.Helpers
{
    public class UploadFile
    {
        static readonly string baseFolder = "Images";
        static readonly string rootUrl = "http://localhost:5185/";
        public static async Task<string> SaveImage(string subFolder, IFormFile formFile)
        {
            // Kiểm tra loại tệp
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(formFile.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Invalid file type. Only image files are allowed.");
            }

            string imageName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), baseFolder, subFolder);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
            var exactPath = Path.Combine(imagePath, imageName);
            using (var fileStream = new FileStream(exactPath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            return rootUrl + Path.Combine(baseFolder, subFolder, imageName).Replace("\\", "/");
        }

        public static void DeleteImage(string urlImage)
        {
            try
            {
                // Lấy đường dẫn file thực tế từ URL
                var relativePath = urlImage.Substring(rootUrl.Length).Replace("/", "\\");
                var exactPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                // Kiểm tra và xóa file nếu tồn tại
                if (File.Exists(exactPath))
                {
                    File.Delete(exactPath);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần (ghi log hoặc bỏ qua)
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }
    };
}
