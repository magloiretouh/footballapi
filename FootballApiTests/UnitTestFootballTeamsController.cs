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
    public class UnitTestFootballTeamsController
    {
        private readonly FootballApiContext context;
        private readonly FootballTeamsController footballTeamsController;

        public UnitTestFootballTeamsController()
        {
            var options = new DbContextOptionsBuilder<FootballApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            context = new FootballApiContext(options);

            context.FootballTeam.Add(new FootballTeam { Name = "Real Madrid Club de Fútbol", Location = "Madrid", ChampionshipID = 1 });
            context.FootballTeam.Add(new FootballTeam { Name = "FC Barcelone", Location = "Barcelone", ChampionshipID = 1 });
            context.FootballTeam.Add(new FootballTeam { Name = "Club Atlético de Madrid", Location = "Madrid", ChampionshipID = 1 });
            context.SaveChanges();

            footballTeamsController = new FootballTeamsController(context);
        }

        [Fact]
        public void GetAllTeams_ShouldReturnAllTeams()
        {
            // Arrange

            // Act
            List<FootballTeam> teams = footballTeamsController.GetFootballTeam().ToList();

            // Assert
            Assert.Equal(3, teams.Count);
        }

        [Fact]
        public void GetTeamById_ShouldReturnTheRightTeam()
        {

            // Arrange
            int teamId = context.FootballTeam.First<FootballTeam>().FootballTeamID;

            // Act
            var okResult = footballTeamsController.GetFootballTeam(teamId).Result as OkObjectResult;

            // Assert
            Assert.IsType<FootballTeam>(okResult.Value);
            Assert.Equal(teamId, (okResult.Value as FootballTeam).FootballTeamID);
        }

        [Fact]
        public void GetTeamByWrongId_ShouldReturnNotFoundResult()
        {

            // Arrange
            int teamId = 1000;

            // Act
            var notFoundResult = footballTeamsController.GetFootballTeam(teamId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PutTeam_ShouldModifyAnItem()
        {
            // Arrange
            var team = context.FootballTeam.First<FootballTeam>();
            team.Name = "Modified Team Name (updated)";

            // Act
            var noContentResult = footballTeamsController.PutFootballTeam(team.FootballTeamID, team);

            // Assert
            Assert.IsType<NoContentResult>(noContentResult.Result);
        }

        [Fact]
        public void PutTeamDifferntIds_ShouldReturnBadRequest()
        {
            // Arrange
            int teamId = 100100;
            var team = new FootballTeam()
            {
                FootballTeamID = 4,
                Name = "Real Madrid Club de Fútbol",
                Location = "Madrid",
                ChampionshipID = 1
            };

            // Act
            var badRequestResult = footballTeamsController.PutFootballTeam(teamId, team);

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult.Result);
        }

        [Fact]
        public void PutTeamInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int teamId = 100100;
            var team = new FootballTeam()
            {
                FootballTeamID = 100100,
                Name = "Real Madrid Club de Fútbol",
                Location = "Madrid",
                ChampionshipID = 1
            };

            // Act
            var notFoundResult = footballTeamsController.PutFootballTeam(teamId, team);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PostTeam_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var team = new FootballTeam()
            {
                FootballTeamID = 102,
                Name = "Celta de Vigo",
                Location = "Vigo",
                ChampionshipID = 1
            };

            // Act
            var createdResponse = footballTeamsController.PostFootballTeam(team);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
        }

        [Fact]
        public void PostTeamInvalidData_ShouldReturnBadRequest()
        {
            // arrange
            var team = new FootballTeam()
            {
                FootballTeamID = 102,
                Name = "Celta de Vigo",
                Location = "Vigo"
            };
            footballTeamsController.ModelState.AddModelError("ChampionshipID", "Required");

            // act
            var badresponse = footballTeamsController.PostFootballTeam(team);

            // assert
            Assert.IsType<BadRequestObjectResult>(badresponse.Result);
        }

        [Fact]
        public void DeleteTeam_ShouldReturnOk()
        {
            // Arrange
            var teamID = context.FootballTeam.First<FootballTeam>().FootballTeamID;

            // Act
            var okResponse = footballTeamsController.DeleteFootballTeam(teamID);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result);
        }

        [Fact]
        public void DeleteTeamInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var notExistingTeamID = 100100;

            // Act
            var badResponse = footballTeamsController.DeleteFootballTeam(notExistingTeamID);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse.Result);
        }
    }
}
