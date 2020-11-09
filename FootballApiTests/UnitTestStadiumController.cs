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
    public class UnitTestStadiumController
    {
        private readonly FootballApiContext context;
        private readonly StadiumController stadiumController;

        public UnitTestStadiumController()
        {
            var options = new DbContextOptionsBuilder<FootballApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            context = new FootballApiContext(options);

            context.Stadium.Add(new Stadium { Name = "Stade Santiago-Bernabéu", NumberOfPlaces = 81044, FootballTeamID = 1 });
            context.Stadium.Add(new Stadium { Name = "Allianz Arena", NumberOfPlaces = 75024, FootballTeamID = 16 });
            context.Stadium.Add(new Stadium { Name = "Juventus Stadium", NumberOfPlaces = 41507, FootballTeamID = 111 });
            context.SaveChanges();

            stadiumController = new StadiumController(context);
        }

        [Fact]
        public void GetAllStadiums_ShouldReturnAllStadiums()
        {
            // Arrange

            // Act
            List<Stadium> stadiums = stadiumController.GetStadium().ToList();

            // Assert
            Assert.Equal(3, stadiums.Count);
        }

        [Fact]
        public void GetStadiumById_ShouldReturnTheRightStadium()
        {

            // Arrange
            int stadiumId = context.Stadium.First<Stadium>().StadiumID;

            // Act
            var okResult = stadiumController.GetStadium(stadiumId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Stadium>(okResult.Value);
            Assert.Equal(stadiumId, (okResult.Value as Stadium).StadiumID);
        }

        [Fact]
        public void GetStadiumByWrongId_ShouldReturnNotFoundResult()
        {

            // Arrange
            int stadiumId = 1000;

            // Act
            var notFoundResult = stadiumController.GetStadium(stadiumId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PutStadium_ShouldModifyAnItem()
        {
            // Arrange
            var stadium = context.Stadium.First<Stadium>();
            stadium.Name = "Modified Stadium Name (updated)";

            // Act
            var noContentResult = stadiumController.PutStadium(stadium.StadiumID, stadium);

            // Assert
            Assert.IsType<NoContentResult>(noContentResult.Result);
        }

        [Fact]
        public void PutStadiumDifferntIds_ShouldReturnBadRequest()
        {
            // Arrange
            int stadiumId = 100100;
            var stadium = new Stadium()
            {
                StadiumID = 4,
                Name = "Stade Santiago-Bernabéu",
                NumberOfPlaces = 81044,
                FootballTeamID = 1
            };

            // Act
            var badRequestResult = stadiumController.PutStadium(stadiumId, stadium);

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult.Result);
        }

        [Fact]
        public void PutStadiumInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int stadiumId = 100100;
            var stadium = new Stadium()
            {
                StadiumID = 100100,
                Name = "Stade Santiago-Bernabéu",
                NumberOfPlaces = 81044,
                FootballTeamID = 1
            };

            // Act
            var notFoundResult = stadiumController.PutStadium(stadiumId, stadium);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PostStadium_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var stadium = new Stadium()
            {
                Name = "Old Trafford",
                NumberOfPlaces = 74879,
                FootballTeamID = 6
            };

            // Act
            var createdResponse = stadiumController.PostStadium(stadium);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
        }

        [Fact]
        public void PostStadiumInvalidData_ShouldReturnBadRequest()
        {
            // arrange
            var stadium = new Stadium()
            {
                Name = "Old Trafford",
                NumberOfPlaces = 74879
            };
            
            stadiumController.ModelState.AddModelError("FootballTeamID", "Required");

            // act
            var badresponse = stadiumController.PostStadium(stadium);

            // assert
            Assert.IsType<BadRequestObjectResult>(badresponse.Result);
        }

        [Fact]
        public void DeleteStadium_ShouldReturnOk()
        {
            // Arrange
            var stadiumID = context.Stadium.First<Stadium>().StadiumID;

            // Act
            var okResponse = stadiumController.DeleteStadium(stadiumID);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result);
        }

        [Fact]
        public void DeleteStadiumInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var notExistingStadiumID = 100100;

            // Act
            var badResponse = stadiumController.DeleteStadium(notExistingStadiumID);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse.Result);
        }
    }
}
