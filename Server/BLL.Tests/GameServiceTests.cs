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
            _dbMock.Setup(x => x.GameRepository.GetAllWithDetailsAsync(It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(games);
            var userService = new GameService(_dbMock.Object,_mapper);

            //act
            var actual = (await userService.GetAllAsync(null, null)).ToList();
            
            //assert
            actual.Should().BeEquivalentTo(expected, options => 
                options.Excluding(x => x.GenreIds));
        }

        [Theory]
        [InlineData("The witcher")]
        public async Task GetAllAsync_WithSorting_ShouldReturnModelsWithSuchName(string name)
        {
            //Arrange
            var fixture = new Fixture();
            var expected = fixture.Build<GameModel>()
                                  .With(game => game.Title, name)
                                  .CreateMany().ToList();
            var games = _mapper.Map<IEnumerable<Game>>(expected);
            _dbMock.Setup(x => x.GameRepository.GetAllWithDetailsAsync(It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync(games);
            var userService = new GameService(_dbMock.Object, _mapper);

            //act
            var actual = (await userService.GetAllAsync(null, "test")).ToList();

            //assert
            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.GenreIds));
        }

        [Fact]
        public async Task GetAllAsync_ThrowGameStoreException()
        {
            //Arrange
            var gamesEmpty = Enumerable.Empty<Game>();
            _dbMock.Setup(x => x.GameRepository.GetAllAsync())
                   .ReturnsAsync(gamesEmpty);
            var userService = new GameService(_dbMock.Object, _mapper);

            //act
            Func<Task> act = async () => await userService.GetAllAsync(null, null);

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
            _dbMock.Setup(x => x.GameRepository.GetByIdWithDetailsAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(game);
            var userService = new GameService(_dbMock.Object, _mapper);

            //act
            var actual = (await userService.GetByIdAsync(id));

            //assert
            actual.Should().BeEquivalentTo(expected, options =>
                options.Excluding(x => x.GenreIds));
            actual.Id.Should().Be(expected.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ThrowException()
        {
            //Arrange
            _dbMock.Setup(x => x.GameRepository.GetByIdAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(() => null);
            var userService = new GameService(_dbMock.Object, _mapper);

            //act
            Func<Task> act = async () => await userService.GetByIdAsync(Guid.NewGuid());

            //assert
            await act.Should().ThrowExactlyAsync<GameStoreException>();
        }

        [Fact]
        public async Task AddAsync_WithoutGenres_ShouldFinishWithSaveAsync()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToAdd = fixture.Build<GameModel>()
                                    .Without(x => x.GenreIds)
                                    .Create();
            _dbMock.Setup(x => x.GameRepository.AddAsync(It.IsAny<Game>()))
                   .Verifiable();
            _dbMock.Setup(x => x.GameGenreRepository.AddRangeAsync(It.IsAny<IEnumerable<GameGenre>>()));
            var gameService = new GameService(_dbMock.Object, _mapper);

            //act
            await gameService.AddAsync(modelToAdd);

            //assert
            _dbMock.Verify(x => x.GameGenreRepository.AddRangeAsync(It.IsAny<IEnumerable<GameGenre>>()),
                Times.Never);
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_WithGenres_ShouldFinishWithSaveAsyncAndCallAddRangeAsync()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToAdd = fixture.Build<GameModel>()
                                       .Create();
            _dbMock.Setup(x => x.GameRepository.AddAsync(It.IsAny<Game>()))
                   .Verifiable();
            _dbMock.Setup(x => x.GameGenreRepository.AddRangeAsync(It.IsAny<IEnumerable<GameGenre>>()));
            var gameService = new GameService(_dbMock.Object, _mapper);

            //act
            await gameService.AddAsync(modelToAdd);

            //assert
            _dbMock.Verify(x => x.GameGenreRepository.AddRangeAsync(It.IsAny<IEnumerable<GameGenre>>()),
                Times.Once);
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithoutGenres_ShouldFinishWithSaveAsync()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToUpdate = fixture.Build<GameModel>()
                                       .Without(x => x.GenreIds)
                                       .Create();
            var moqGameFromDb = fixture.Build<Game>()
                                       .Without(x => x.Comments)
                                       .Without(x => x.GameGenres)
                                       .Without(x => x.GameImages)
                                       .Create();
            _dbMock.Setup(x => x.GameRepository.GetByIdWithDetailsWithNoTrack(It.IsAny<Guid>()))
                   .ReturnsAsync(moqGameFromDb);
            _dbMock.Setup(x => x.GameRepository.Update(It.IsAny<Game>()))
                   .Verifiable();
            _dbMock.Setup(x => x.GameGenreRepository.AddRangeAsync(It.IsAny<IEnumerable<GameGenre>>()));
            var gameService = new GameService(_dbMock.Object, _mapper);

            //act
            await gameService.UpdateAsync(modelToUpdate);

            //assert
            _dbMock.Verify(x => x.GameGenreRepository.AddRangeAsync(It.IsAny<IEnumerable<GameGenre>>()),
                Times.Never);
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithGenres_ShouldFinishWithSaveAsync()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToUpdate = fixture.Build<GameModel>()
                                       .Create();
            var moqGameFromDb = fixture.Build<Game>()
                                       .Without(x => x.Comments)
                                       .Without(x => x.GameGenres)
                                       .Without(x => x.GameImages)
                                       .Create();
            var moqGameGenres = fixture
                                .Build<GameGenre>()
                                .Without(x => x.Game)
                                .Without(x => x.Genre)
                                .CreateMany<GameGenre>();
            _dbMock.Setup(x => x.GameRepository.GetByIdWithDetailsWithNoTrack(It.IsAny<Guid>()))
                   .ReturnsAsync(moqGameFromDb);
            _dbMock.Setup(x => x.GameRepository.Update(It.IsAny<Game>()))
                   .Verifiable();
            _dbMock.Setup(x => x.GameGenreRepository.GetAllAsync())
                   .ReturnsAsync(moqGameGenres);

            var gameService = new GameService(_dbMock.Object, _mapper);

            //act
            await gameService.UpdateAsync(modelToUpdate);

            //assert
            _dbMock.Verify(x => x.GameGenreRepository.DeleteRange(It.IsAny<IEnumerable<GameGenre>>()),
                Times.Once);
            _dbMock.Verify(x => x.GameGenreRepository.AddRangeAsync(It.IsAny<IEnumerable<GameGenre>>()),
                Times.Once);
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithoutGenres_ShouldFinishWithSaveAsync()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToAdd = fixture.Build<GameModel>()
                                    .Without(x => x.GenreIds)
                                    .Create();
            _dbMock.Setup(x => x.GameRepository.Delete(It.IsAny<Game>()))
                   .Verifiable();
            var gameService = new GameService(_dbMock.Object, _mapper);

            //act
            await gameService.DeleteAsync(modelToAdd);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithGenres_ShouldFinishWithSaveAsync()
        {
            //Arrange
            var fixture = new Fixture();
            var modelToAdd = fixture.Build<GameModel>()
                                    .Create();
            _dbMock.Setup(x => x.GameRepository.Delete(It.IsAny<Game>()))
                   .Verifiable();
            var gameService = new GameService(_dbMock.Object, _mapper);

            //act
            await gameService.DeleteAsync(modelToAdd);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}