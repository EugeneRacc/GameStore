using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public OrderService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<OrderDetailsModel>> GetAllOrdersByUserIdAsync(Guid userId)
    {
        var orders = (await _unitOfWork.OrderDetailsRepository.GetOrdersByUserId(userId)).ToList();
        return _mapper.Map<IEnumerable<OrderDetailsModel>>(orders);
    }

    public async Task<OrderDetailsModel> AddOrderAsync(OrderDetailsModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null || user.FirstName != model.FirstName
                         || user.LastName != model.LastName)
            throw new GameStoreException("User not found or information is invalid");
        model.Id = Guid.NewGuid();
        model.OrderDate = DateTime.Now;
        foreach (var game in model.OrderedGames)
        {
            game.OrderDetailsId = model.Id;
        }
        await _unitOfWork.OrderDetailsRepository.AddAsync(_mapper.Map<OrderDetails>(model));
        await _unitOfWork.SaveAsync();
        return model;
    }
}