using System;
using System.Linq;
using Xunit;
using FootballApi.Models;
using FootballApi.Controllers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FootballApiTests
{
    public class UnitTestChampionshipsController : IDisposable
    {
        private readonly FootballApiContext context;
        private readonly ChampionshipsController championshipsController;

        public UnitTestChampionshipsController()
        {
            // construc
            var options = new DbContextOptionsBuilder<FootballApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            context = new FootballApiContext(options);

            context.Championship.Add(new Championship { Name = "LaLiga Santander" });
            context.Championship.Add(new Championship { Name = "Premier League" });
            context.Championship.Add(new Championship { Name = "Serie A" });
            context.SaveChanges();

            championshipsController = new ChampionshipsController(context);
        }

        public void Dispose()
        {
            // Do some cleanup for every test
        }

        [Fact]
        public void GetAllChampionships_ShouldReturnAllChampionships()
        {
            // Arrange

            // Act
            List<Championship> championships = championshipsController.GetChampionship().ToList();

            // Assert
            Assert.Equal(3, championships.Count);
        }

        [Fact]
        public void GetChampionshipById_ShouldReturnTheRightChampionship()
        {

            // Arrange
            int championshipId = context.Championship.First<Championship>().ChampionshipID;

            // Act
            var okResult = championshipsController.GetChampionship(championshipId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Championship>(okResult.Value);
            Assert.Equal(championshipId, (okResult.Value as Championship).ChampionshipID);
        }

        [Fact]
        public void GetChampionshipByWrongId_ShouldReturnNotFoundResult()
        {

            // Arrange
            int championshipId = 1000;

            // Act
            var notFoundResult = championshipsController.GetChampionship(championshipId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PutChampionship_ShouldModifyAnItem()
        {

            // Arrange
            var championship = context.Championship.First<Championship>();
            championship.Name = "Modified Championship Name (updated)";

            // Act
            var noContentResult = championshipsController.PutChampionship(championship.ChampionshipID, championship);

            // Assert
            Assert.IsType<NoContentResult>(noContentResult.Result);
        }

        [Fact]
        public void PutChampionshipDifferntIds_ShouldReturnBadRequest()
        {
            // Arrange
            int championshipId = 100100;
            var championship = new Championship()
            {
                ChampionshipID = 4,
                Name = "LaLiga Santander (Updated)"
            };

            // Act
            var badRequestResult = championshipsController.PutChampionship(championshipId, championship);

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult.Result);
        }

        [Fact]
        public void PutChampionshipInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int championshipId = 100100;
            var championship = new Championship()
            {
                ChampionshipID = 100100,
                Name = "LaLiga Santander (Updated)"
            };

            // Act
            var notFoundResult = championshipsController.PutChampionship(championshipId, championship);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PostChampionship_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var championship = new Championship()
            {
                ChampionshipID = 1002,
                Name = "Bundesliga"
            };

            // Act
            var createdResponse = championshipsController.PostChampionship(championship);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
        }

        [Fact]
        public void PostChampionshipInvalidData_ShouldReturnBadRequest()
        {
            // arrange
            var championship = new Championship()
            {
                ChampionshipID = 1002,
                Name = "Bundesliga"
            };
            championshipsController.ModelState.AddModelError("FootballTeams", "Required");

            // act
            var badresponse = championshipsController.PostChampionship(championship);

            // assert
            Assert.IsType<BadRequestObjectResult>(badresponse.Result);
        }

        [Fact]
        public void DeleteChampionship_ShouldReturnOk()
        {
            // Arrange
            var championshipID = context.Championship.First<Championship>().ChampionshipID;

            // Act
            var okResponse = championshipsController.DeleteChampionship(championshipID);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result);
        }

        [Fact]
        public void DeleteChampionshipInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var notExistingChampionshipID = 100100;

            // Act
            var badResponse = championshipsController.DeleteChampionship(notExistingChampionshipID);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse.Result);
        }
    }
}
