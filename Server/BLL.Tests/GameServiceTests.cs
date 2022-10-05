using BLL.Models;
using BLL.Services;
using DAL.Entities;
using Moq;
using AutoFixture;
using AutoMapper;
using BLL.Exceptions;
using DAL.Interfaces;
using FluentAssertions;
using BLL.Mapper;

namespace BLL.Tests
{
    public class GameServiceTests
    {
        private readonly Mock<IUnitOfWork> _dbMock = new Mock<IUnitOfWork>();
        private readonly IMapper _mapper;

        public GameServiceTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = new AutoMapper.Mapper(configuration);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGameModels()
        {
            //Arrange
            var fixture = new Fixture();
            var expected = fixture.Build<GameModel>().CreateMany().ToList();
            var games = _mapper.Map<IEnumerable<Game>>(expected);
            _dbMock.Setup(x => x.GameRepository.GetAllAsync())
                   .Returns(() => Task.FromResult(games));
            var userService = new GameService(_dbMock.Object,_mapper);

            //act
            var actual = (await userService.GetAllAsync()).ToList();
            
            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAllAsync_ThrowGameStoreException()
        {
            //Arrange
            var gamesEmpty = Enumerable.Empty<Game>();
            _dbMock.Setup(x => x.GameRepository.GetAllAsync())
                   .Returns(() => Task.FromResult(gamesEmpty));
            var userService = new GameService(_dbMock.Object, _mapper);

            //act
            Func<Task> act = async () => await userService.GetAllAsync();

            //assert
            await act.Should().ThrowExactlyAsync<GameStoreException>();
        }

        [Theory]
        [InlineData("2771a499-0254-4787-91e9-aa04d6483b6c")]
        [InlineData("79c4c9a3-3366-4f48-b602-7f6442202b4b")]
        [InlineData("ee6b5c82-d57f-432f-aa35-124b158bc0f9")]
        public async Task GetByIdAsync_GetOneGame(Guid id)
        {
            //Arrange
            var fixture = new Fixture();
            var expected = fixture.Build<GameModel>()
                                  .With(model => model.Id, id)
                                  .Create();
            var game = _mapper.Map<Game>(expected);
            _dbMock.Setup(x => x.GameRepository.GetByIdAsync(It.IsAny<Guid>()))
                   .Returns(() => Task.FromResult(game));
            var userService = new GameService(_dbMock.Object, _mapper);

            //act
            var actual = (await userService.GetByIdAsync(id));

            //assert
            actual.Should().BeEquivalentTo(expected);
            actual.Id.Should().Be(expected.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowException()
        {
            //Arrange
            _dbMock.Setup(x => x.GameRepository.GetByIdAsync(It.IsAny<Guid>()))
                   .Returns(Task.FromResult<Game>(null));
            var userService = new GameService(_dbMock.Object, _mapper);

            //act
            Func<Task> act = async () => await userService.GetByIdAsync(Guid.NewGuid());

            //assert
            await act.Should().ThrowExactlyAsync<GameStoreException>();
        }

    }
}