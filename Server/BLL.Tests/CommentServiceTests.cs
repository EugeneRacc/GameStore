using AutoFixture;
using AutoMapper;
using BLL.Exceptions;
using BLL.Mapper;
using BLL.Models;
using BLL.Services;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BLL.Tests
{
    public class CommentServiceTests
    {
        private readonly Mock<IUnitOfWork> _dbMock = new Mock<IUnitOfWork>();
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;

        public CommentServiceTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = new AutoMapper.Mapper(configuration);
            _userManagerMock = TestHelpers.MockUserManager<User>();
            _roleManagerMock = TestHelpers.MockRoleManager<IdentityRole>();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCommentModels()
        {
            //Arrange
            var fixture = new Fixture();
            var expected = fixture.Build<CommentModel>().CreateMany().ToList();
            var games = _mapper.Map<IEnumerable<Comment>>(expected);
            _dbMock.Setup(x => x.CommentRepository.GetAllAsync())
                   .ReturnsAsync(games);
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act
            var actual = (await commentService.GetAllAsync()).ToList();

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByGameIdAsync_WithExistingId_ShouldReturnListOfModels()
        {
            //Arrange
            var fixture = new Fixture();
            var neededId = fixture.Create<Guid>();
            var expected = fixture.Build<CommentModel>()
                                  .With(x => x.GameId, neededId)
                                  .CreateMany();
            var games = _mapper.Map<IEnumerable<Comment>>(expected);
            _dbMock.Setup(x => x.CommentRepository.GetByGameIdAsync(neededId))
                   .ReturnsAsync(games);
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act
            var actual = await commentService.GetGameComments(neededId);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByGameIdAsync_WithInvalidId_ShouldReturnListOfModels()
        {
            //Arrange
            var fixture = new Fixture();
            var neededId = fixture.Create<Guid>();
            var expected = Enumerable.Empty<CommentModel>();
            var games = _mapper.Map<IEnumerable<Comment>>(expected);
            _dbMock.Setup(x => x.CommentRepository.GetByGameIdAsync(neededId))
                   .ReturnsAsync(games);
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act
            var actual = await commentService.GetGameComments(neededId);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCommentModel()
        {
            //Arrange
            var fixture = new Fixture();
            var neededId = fixture.Create<Guid>();
            var expected = fixture.Create<CommentModel>();
            var game = _mapper.Map<Comment>(expected);
            _dbMock.Setup(x => x.CommentRepository.GetByIdAsync(neededId))
                   .ReturnsAsync(game);
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act
            var actual = await commentService.GetByIdAsync(neededId);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdAsync_WithInValidId_ShouldThrowException()
        {
            //Arrange
            var fixture = new Fixture();
            var neededId = fixture.Create<Guid>();
            Comment expected = null;
            _dbMock.Setup(x => x.CommentRepository.GetByIdAsync(neededId))
                   .ReturnsAsync(expected);
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act
            Func<Task> act = async () => await commentService.GetByIdAsync(neededId);
            //assert
            await act.Should().ThrowExactlyAsync<GameStoreException>();
        }

        [Fact]
        public async Task AddAsync_WithValidModel_ShouldFinishWithSaveAsyncAndReturnModel()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToAdd = fixture.Build<CommentModel>()
                                    .Without(x => x.CreatedDate)
                                    .Without(x => x.Id)
                                    .Create();
            _dbMock.Setup(x => x.CommentRepository.AddAsync(It.IsAny<Comment>()));
            _dbMock.Setup(x => x.SaveAsync()).Verifiable();
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act
            var actual = await commentService.AddAsync(modelToAdd);

            //assert
            actual.Should().BeEquivalentTo(modelToAdd, options => 
                options.Excluding(x => x.CreatedDate)
                       .Excluding(x => x.Id));
            actual.CreatedDate.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteAsync_WithValidModel_ShouldFinishWithSaveAsync()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToDelete = fixture.Build<CommentModel>()
                                    .Create();
            _dbMock.Setup(x => x.CommentRepository.Delete(It.IsAny<Comment>()));
            _dbMock.Setup(x => x.SaveAsync()).Verifiable();
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act & assert
            await commentService.DeleteAsync(modelToDelete);
        }

        [Fact]
        public async Task UpdateAsync_WithValidModel_ShouldFinishWithSaveAsync()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToUpdate = fixture.Build<CommentModel>()
                                       .Create();
            var existedComment = _mapper.Map<Comment>(modelToUpdate);
            _dbMock.Setup(x => x.CommentRepository.GetByIdAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(existedComment);
            _dbMock.Setup(x => x.CommentRepository.Update(It.IsAny<Comment>()));
            _dbMock.Setup(x => x.SaveAsync()).Verifiable();
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act & assert
            await commentService.UpdateAsync(modelToUpdate);
        }

        [Fact]
        public async Task UpdateAsync_WithInValidModel_ShouldThrowException()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToUpdate = fixture.Build<CommentModel>()
                                       .Create();
            Comment existedComment = null;
            _dbMock.Setup(x => x.CommentRepository.GetByIdAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(existedComment);
            _dbMock.Setup(x => x.CommentRepository.Update(It.IsAny<Comment>()));
           
            var commentService = new CommentService(_mapper, _dbMock.Object, _userManagerMock.Object);

            //act
            Func<Task> act = async () =>  await commentService.UpdateAsync(modelToUpdate);

            //assert
            await act.Should().ThrowExactlyAsync<GameStoreException>();
        }
    }
}
