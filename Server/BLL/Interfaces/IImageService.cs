using BLL.Models;

namespace BLL.Interfaces
{
    public interface IImageService
    {
        public Task<ImageModel> UploadImageAsync(ImageModel image);
        public Task<IEnumerable<ImageModel>> GetImagesByGameIdAsync(Guid id);
    }
}
