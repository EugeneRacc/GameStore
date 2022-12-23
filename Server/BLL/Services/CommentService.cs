using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public CommentService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public async Task<IEnumerable<CommentModel>> GetAllAsync()
        {
            var comments = await _unitOfWork.CommentRepository.GetAllAsync();
            if (comments == null)
                throw new GameStoreException("Comments not found");
            return _mapper.Map<IEnumerable<CommentModel>>(comments);
        }

        public async Task<CommentModel> GetByIdAsync(Guid id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new GameStoreException($"No Comment with such id: {id}");
            return _mapper.Map<CommentModel>(comment);
        }

        public async Task<CommentModel> AddAsync(CommentModel model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedDate = DateTime.Now;
            await _unitOfWork.CommentRepository.AddAsync(_mapper.Map<Comment>(model));
            await _unitOfWork.SaveAsync();
            return model;
        }

        public async Task<CommentModel> UpdateAsync(CommentModel model)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(model.Id ?? Guid.Empty);
            if (comment == null)
                throw new GameStoreException($"No comment with such id: {model.Id}");
            comment.Body = model.Body;
            _unitOfWork.CommentRepository.Update(comment);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CommentModel>(comment);
        }

        public async Task DeleteAsync(CommentModel model)
        {
            _unitOfWork.CommentRepository.Delete(_mapper.Map<Comment>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(IEnumerable<CommentModel> models)
        {
            
            _unitOfWork.CommentRepository.DeleteRange(_mapper.Map<IEnumerable<Comment>>(models));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(IEnumerable<CommentModel> models, string userId)
        {
            bool isOk = true;
            foreach (var model in models)
            {
                if (model.UserId == Guid.Parse(userId))
                {
                    _unitOfWork.CommentRepository.Delete(_mapper.Map<Comment>(model));
                }
                else
                {
                    isOk = false;
                }
            }

            await _unitOfWork.SaveAsync();
            if (!isOk)
            {
                throw new GameStoreException("No such permissions");
            }
        }

        public async Task<IEnumerable<CommentModel>> GetGameComments(Guid gameId)
        {
            var comments = await _unitOfWork.CommentRepository
                                            .GetByGameIdAsync(gameId);
            return _mapper.Map<IEnumerable<CommentModel>>(comments);
        }
    }
}
