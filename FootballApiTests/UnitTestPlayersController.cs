using System.Linq;
using Xunit;
using FootballApi.Models;
using FootballApi.Controllers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FootballApiTests
{
    public class UnitTestPlayersController
    {
        private readonly FootballApiContext context;
        private readonly PlayersController playersController;

        public UnitTestPlayersController()
        {
            var options = new DbContextOptionsBuilder<FootballApiContext>()
            .UseInMemoryDatabase(databaseName: "PlayerListDatabase")
            .Options;

            context = new FootballApiContext(options);

            context.Player.Add(new Player {
                FullName = "Cristiano Ronaldo dos Santos Aveiro",
                Surname = "Cristiano Ronaldo",
                Height = 187,
                Weight = 87,
                Speed = 95,
                Finishing = 98,
                FreeKick = 96,
                Dribbling = 87,
                FootballTeamID = 1 });
            context.Player.Add(new Player
            {
                FullName = "Lionel Messi",
                Surname = "Leo Messi",
                Height = 170,
                Weight = 72,
                Speed = 94,
                Finishing = 98,
                FreeKick = 96,
                Dribbling = 92,
                FootballTeamID = 1
            });
            context.Player.Add(new Player
            {
                FullName = "Neymar da Silva Santos Júnior",
                Surname = "Neymar",
                Height = 175,
                Weight = 68,
                Speed = 90,
                Finishing = 91,
                FreeKick = 85,
                Dribbling = 95,
                FootballTeamID = 1
            });
            context.SaveChanges();

            playersController = new PlayersController(context);
        }

        [Fact]
        public void GetAllPlayers_ShouldReturnAllPlayers()
        {
            // Arrange

            // Act
            List<Player> players = playersController.GetPlayer().ToList();

            // Assert
            Assert.Equal(3, players.Count);
        }

        [Fact]
        public void GetPlayerById_ShouldReturnTheRightPlayer()
        {

            // Arrange
            int playerId = 3;

            // Act
            var okResult = playersController.GetPlayer(playerId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Player>(okResult.Value);
            Assert.Equal(playerId, (okResult.Value as Player).PlayerID);
        }

        [Fact]
        public void GetPlayerByWrongId_ShouldReturnNotFoundResult()
        {

            // Arrange
            int playerId = 1000;

            // Act
            var notFoundResult = playersController.GetPlayer(playerId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        //[Fact]
        //public void PutPlayer_ShouldAddNewItem()
        //{

        //    // Arrange
        //    int playerId = 1;
        //    var player = new Player()
        //    {
        //        PlayerID = 1,
        //        FullName = "Cristiano Ronaldo dos Santos Aveiro",
        //        Surname = "Cristiano Ronaldo",
        //        Height = 187,
        //        Weight = 87,
        //        Speed = 95,
        //        Finishing = 98,
        //        FreeKick = 96,
        //        Dribbling = 87,
        //        FootballTeamID = 1
        //    };

        //    // Act
        //    var noContentResult = playersController.PutPlayer(playerId, player);

        //    // Assert
        //    Assert.IsType<NoContentResult>(noContentResult.Result);
        //}

        [Fact]
        public void PutPlayerDifferntIds_ShouldReturnBadRequest()
        {
            // Arrange
            int playerId = 100100;
            var player = new Player()
            {
                PlayerID = 4,
                FullName = "Cristiano Ronaldo dos Santos Aveiro",
                Surname = "Cristiano Ronaldo",
                Height = 187,
                Weight = 87,
                Speed = 95,
                Finishing = 98,
                FreeKick = 96,
                Dribbling = 87,
                FootballTeamID = 1
            };

            // Act
            var badRequestResult = playersController.PutPlayer(playerId, player);

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult.Result);
        }

        [Fact]
        public void PutPlayerInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int playerId = 100100;
            var player = new Player()
            {
                PlayerID = 100100,
                FullName = "Cristiano Ronaldo dos Santos Aveiro",
                Surname = "Cristiano Ronaldo",
                Height = 187,
                Weight = 87,
                Speed = 95,
                Finishing = 98,
                FreeKick = 96,
                Dribbling = 87,
                FootballTeamID = 1
            };

            // Act
            var notFoundResult = playersController.PutPlayer(playerId, player);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PostPlayer_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var player = new Player()
            {
                FullName = "Cristiano Ronaldo dos Santos Aveiro",
                Surname = "Cristiano Ronaldo",
                Height = 187,
                Weight = 87,
                Speed = 95,
                Finishing = 98,
                FreeKick = 96,
                Dribbling = 87,
                FootballTeamID = 1
            };

            // Act
            var createdResponse = playersController.PostPlayer(player);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
        }

        [Fact]
        public void PostPlayerInvalidData_ShouldReturnBadRequest()
        {
            // arrange
            var player = new Player()
            {
                FullName = "Cristiano Ronaldo dos Santos Aveiro",
                Surname = "Cristiano Ronaldo",
                Height = 187,
                Weight = 87,
                Speed = 95,
                Finishing = 98,
                FreeKick = 96,
                Dribbling = 87
            };
            playersController.ModelState.AddModelError("FootballTeamID", "Required");

            // act
            var badresponse = playersController.PostPlayer(player);

            // assert
            Assert.IsType<BadRequestObjectResult>(badresponse.Result);
        }

        [Fact]
        public void DeletePlayer_ShouldReturnOk()
        {
            // Arrange
            var playerID = 1;

            // Act
            var okResponse = playersController.DeletePlayer(playerID);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result);
        }

        [Fact]
        public void DeletePlayerInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var notExistingPlayerID = 100100;

            // Act
            var badResponse = playersController.DeletePlayer(notExistingPlayerID);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse.Result);
        }
    }
}
