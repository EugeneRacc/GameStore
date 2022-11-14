using System.Collections.Immutable;
using AutoFixture;
using AutoMapper;
using BLL.Exceptions;
using BLL.Mapper;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BLL.Tests;

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _dbMock = new Mock<IUnitOfWork>();
    private readonly IMapper _mapper;
    private readonly Mock<UserManager<User>> _userManagerMock;

    public OrderServiceTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        _mapper = new AutoMapper.Mapper(configuration);
        _userManagerMock = TestHelpers.MockUserManager<User>();
    }

    [Fact]
    public async Task GetAllOrdersByUserIdAsync_WithCorrectId_ShouldReturnListOfOrderDetailsModels()
    {
        //Arrange
        var fixture = new Fixture();
        var expectedUserId = fixture.Create<Guid>();
        var expected = fixture.Build<OrderDetailsModel>()
            .Without(x => x.Email)
            .With(x => x.UserId, expectedUserId.ToString())
            .CreateMany().ToList();
        var orderDetails = _mapper.Map<IEnumerable<OrderDetails>>(expected);
        _dbMock.Setup(x => x.OrderDetailsRepository.GetOrdersByUserId(expectedUserId))
            .ReturnsAsync(orderDetails);
        var orderService = new OrderService(_mapper, _dbMock.Object, _userManagerMock.Object);

        //act
        var actual = (await orderService.GetAllOrdersByUserIdAsync(expectedUserId));
            
        //assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task GetAllOrdersByUserIdAsync_WithInCorrectId_ShouldReturnEmptyList()
    {
        //Arrange
        var fixture = new Fixture();
        var expectedUserId = fixture.Create<Guid>();
        var expected = Enumerable.Empty<OrderDetailsModel>();
        var emptyOrderDetails = _mapper.Map<IEnumerable<OrderDetails>>(expected);
        _dbMock.Setup(x => x.OrderDetailsRepository.GetOrdersByUserId(expectedUserId))
            .ReturnsAsync(emptyOrderDetails);
        var orderService = new OrderService(_mapper, _dbMock.Object, _userManagerMock.Object);

        //act
        var actual = (await orderService.GetAllOrdersByUserIdAsync(expectedUserId));
            
        //assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task AddOrder_WithInCorrectUserId_ShouldThrowException()
    {
        //Arrange
        var fixture = new Fixture();
        var expectedUserId = fixture.Create<Guid>();
        User userWithNeededId = null;
        var orderToAdd = fixture
            .Build<OrderDetailsModel>()
            .With(x => x.UserId, expectedUserId.ToString())
            .Create();
        _userManagerMock.Setup(x => x.FindByIdAsync(expectedUserId.ToString()))
            .ReturnsAsync(userWithNeededId);
        var orderService = new OrderService(_mapper, _dbMock.Object, _userManagerMock.Object);

        //act
        Func<Task> actual = async () => await orderService.AddOrderAsync(orderToAdd);
            
        //assert
        await actual.Should().ThrowExactlyAsync<GameStoreException>();
    }
    
    [Fact]
    public async Task AddOrder_WithIncorrectName_ShouldThrowException()
    {
        //Arrange
        var fixture = new Fixture();
        var expectedUserId = fixture.Create<Guid>();
        var userWithNeededId = fixture.Build<UserModel>()
            .With(x => x.Id, expectedUserId)
            .Create();
        var mappedUser = _mapper.Map<User>(userWithNeededId);
        var orderToAdd = fixture.Build<OrderDetailsModel>()
            .With(x => x.UserId, expectedUserId.ToString())
            .Create();
        _userManagerMock.Setup(x => x.FindByIdAsync(orderToAdd.UserId))
            .ReturnsAsync(mappedUser);
        var orderService = new OrderService(_mapper, _dbMock.Object, _userManagerMock.Object);

        //act
        Func<Task> actual = async () => await orderService.AddOrderAsync(orderToAdd);
            
        //assert
        await actual.Should().ThrowExactlyAsync<GameStoreException>();
    }
    
    [Fact]
    public async Task AddOrder_WithCorrectModel_ShouldSaveAsync()
    {
        //Arrange
        var fixture = new Fixture();
        var expectedUserId = fixture.Create<Guid>();
        var userWithNeededId = fixture.Build<UserModel>()
            .With(x => x.Id, expectedUserId)
            .Create();
        var mappedUser = _mapper.Map<User>(userWithNeededId);
        var orderToAdd = fixture.Build<OrderDetailsModel>()
            .With(x => x.UserId, expectedUserId.ToString())
            .With(x => x.FirstName, userWithNeededId.FirstName)
            .With(x => x.LastName, userWithNeededId.LastName)
            .Create();
        _userManagerMock.Setup(x => x.FindByIdAsync(orderToAdd.UserId))
            .ReturnsAsync(mappedUser);
        _dbMock.Setup(x => x.OrderDetailsRepository
                .AddAsync(_mapper.Map<OrderDetails>(orderToAdd)))
            .Verifiable();
        var orderService = new OrderService(_mapper, _dbMock.Object, _userManagerMock.Object);

        //act
        await orderService.AddOrderAsync(orderToAdd);
        //assert
        
        _dbMock.Verify(x => x.SaveAsync(), Times.Once);
    }
}