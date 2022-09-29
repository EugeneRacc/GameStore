using BLL.Models;
using BLL.Services;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System;
using System.Collections;
using BLL.Exceptions;
using DAL.Interfaces;
using BLL.Tests.Helpers;

namespace BLL.Tests
{
    public class GameServiceTests
    {
        public readonly Mock<IUnitOfWork> _dbMock = new Mock<IUnitOfWork>();
        private readonly GameListEqualityComparer equalityList = new GameListEqualityComparer();
        private readonly GameEqualityComparer equalityGame = new GameEqualityComparer();

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGameModels()
        {
            //Arrange
            var expected = GetTestGameModels;

            _dbMock.Setup(x => x.GameRepository.GetAllAsync())
                   .Returns(ReturnGames());

            var userService = new GameService(_dbMock.Object, SeedData.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllAsync()).ToList();
            
            //assert
            Assert.True(equalityList.Equals(expected, actual));

        }

        [Fact]
        public async Task GetAllAsync_ThrowGameStoreException()
        {
            //Arrange
            List<GameModel> expected = null;

            _dbMock.Setup(x => x.GameRepository.GetAllAsync())
                   .Returns(GetInvalidData());

            var userService = new GameService(_dbMock.Object, SeedData.CreateMapperProfile());

            //act
            Func<Task> act = async () => (await userService.GetAllAsync()).ToList();

            //assert
            await Assert.ThrowsAsync<GameStoreException>(act);

        }

        [Fact]
        public async Task GetByIdAsync_GetOneGame()
        {
            //Arrange
            var expected = GetTestGameModels[0];

            _dbMock.Setup(x => x.GameRepository.GetByIdAsync(It.IsAny<Guid>()))
                   .Returns(GetOneTestGame());

            var userService = new GameService(_dbMock.Object, SeedData.CreateMapperProfile());

            //act
            var actual = (await userService.GetByIdAsync(Guid.Parse("2771a499-0254-4787-91e9-aa04d6483b6c")));

            //assert
            Assert.True(equalityGame.Equals(expected, actual));

        }

        [Fact]
        public async Task GetByIdAsync_ThrowException()
        {
            //Arrange
            List<GameModel> expected = null;

            _dbMock.Setup(x => x.GameRepository.GetByIdAsync(It.IsAny<Guid>()))
                   .Returns((async () => null));

            var userService = new GameService(_dbMock.Object, SeedData.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.GetByIdAsync(Guid.Parse("2771a499-0254-4787-91e9-aa04d6483b6c"));

            //assert
            await Assert.ThrowsAsync<GameStoreException>(act);

        }

        #region Utility

        private async Task<IEnumerable<Game>> ReturnGames()
        {
            return new List<Game>()
            {
                new Game { Id = Guid.Parse("2771a499-0254-4787-91e9-aa04d6483b6c"), Title = "The Witcher", Description = "Good Rpg", Price = 130.0m },
                new Game { Id = Guid.Parse("6652626f-84da-4b22-b990-c2d428eedf8d"), Title = "The Witcher 1", Description = "Good Rpg", Price = 175.0m },
                new Game { Id = Guid.Parse("79c4c9a3-3366-4f48-b602-7f6442202b4b"), Title = "The Witcher 2", Description = "Good Rpg", Price = 99.9m },
                new Game { Id = Guid.Parse("a91c62ef-3a0a-493a-8e7c-ec9b5043e01c"), Title = "The Witcher 3", Description = "Good Rpg", Price = 270.0m },
                new Game { Id = Guid.Parse("08952dc8-a89a-4d06-99ab-76624ffe76fc"), Title = "The Witcher 4", Description = "Good Rpg", Price = 130.0m },
                new Game { Id = Guid.Parse("ee6b5c82-d57f-432f-aa35-124b158bc0f9"), Title = "The Witcher 5", Description = "Good Rpg", Price = 130.0m },
                new Game { Id = Guid.Parse("2a919e12-aaef-406f-bb9f-0f89d151f316"), Title = "The Witcher 6", Description = "Good Rpg", Price = 130.0m },
            }.AsEnumerable();
        }

        private List<GameModel> GetTestGameModels => new List<GameModel>()
        {
            new GameModel { Id = Guid.Parse("2771a499-0254-4787-91e9-aa04d6483b6c"), Title = "The Witcher", Description = "Good Rpg", Price = 130.0m },
            new GameModel { Id = Guid.Parse("6652626f-84da-4b22-b990-c2d428eedf8d"), Title = "The Witcher 1", Description = "Good Rpg", Price = 175.0m },
            new GameModel { Id = Guid.Parse("79c4c9a3-3366-4f48-b602-7f6442202b4b"), Title = "The Witcher 2", Description = "Good Rpg", Price = 99.9m },
            new GameModel { Id = Guid.Parse("a91c62ef-3a0a-493a-8e7c-ec9b5043e01c"), Title = "The Witcher 3", Description = "Good Rpg", Price = 270.0m },
            new GameModel { Id = Guid.Parse("08952dc8-a89a-4d06-99ab-76624ffe76fc"), Title = "The Witcher 4", Description = "Good Rpg", Price = 130.0m },
            new GameModel { Id = Guid.Parse("ee6b5c82-d57f-432f-aa35-124b158bc0f9"), Title = "The Witcher 5", Description = "Good Rpg", Price = 130.0m },
            new GameModel { Id = Guid.Parse("2a919e12-aaef-406f-bb9f-0f89d151f316"), Title = "The Witcher 6", Description = "Good Rpg", Price = 130.0m }
        };

        private async Task<IEnumerable<Game>> GetInvalidData()
        {
            return new List<Game>();
        }
        private async Task<Game> GetOneTestGame()
        {
            return new Game
            {
                Id = Guid.Parse("2771a499-0254-4787-91e9-aa04d6483b6c"), Title = "The Witcher",
                Description = "Good Rpg", Price = 130.0m
            };

        }
        #endregion
    }
}