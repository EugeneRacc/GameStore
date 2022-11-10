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

    //TODO Not forget to implement add logic
    public async Task<OrderModel> AddOrder(OrderModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null || user.FirstName != model.FirstName
                         || user.LastName != model.LastName)
            throw new GameStoreException("User not found or information is invalid");
        return model;
    }
}