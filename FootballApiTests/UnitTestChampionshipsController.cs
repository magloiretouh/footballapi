using System;
using System.Linq;
using Xunit;
using FootballApi.Models;
using FootballApi.Controllers;

namespace FootballApiTests
{
    public class UnitTestChampionshipsController
    {
        private readonly FootballApiContext _context;
        ChampionshipsController _controller;

        public UnitTestChampionshipsController()
        {
            _controller = new ChampionshipsController(_context);
        }

        [Fact]
        public void GetAllChampionships_ShouldReturnAllProducts()
        {
            // arrange
            var numberOfChampionship = 5;

            // act
            var results = _controller.GetChampionship();

            // assert
            Assert.Equal(numberOfChampionship, results.Count());
        }

    }
}
