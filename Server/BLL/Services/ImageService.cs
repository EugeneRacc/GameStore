using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public ImageService(IMapper mapper, IUnitOfWork db)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ImageModel> UploadImageAsync(ImageModel image)
        {
            if (image == null)
                throw new GameStoreException("Image doesn't exist");
            image.Id = Guid.NewGuid();
            await _db.GameImage.AddAsync(_mapper.Map<GameImage>(image));
            await _db.SaveAsync();
            return image;
        }

        public async Task<IEnumerable<ImageModel>> GetImagesByGameIdAsync(Guid id)
        {
            var images = await _db.GameImage.GetImagesByGameId(id);
            if (images == null)
                throw new GameStoreException("No images with this game id");
            return _mapper.Map<IEnumerable<ImageModel>>(images);
        }

        public async Task DeleteAsync(ImageModel model)
        {
            _db.GameImage.Delete(_mapper.Map<GameImage>(model));
            await _db.SaveAsync();
        }
    }
}
