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
    public class UnitTestTransactionsController
    {
        private readonly FootballApiContext context;
        private readonly TransactionsController transactionsController;

        public UnitTestTransactionsController()
        {
            var options = new DbContextOptionsBuilder<FootballApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            context = new FootballApiContext(options);

            context.Transaction.Add(new Transaction { PlayerID = 1, LeavingTeamID = 6, ComingTeamID = 1 });
            context.Transaction.Add(new Transaction { PlayerID = 1, LeavingTeamID = 1, ComingTeamID = 11 });
            context.Transaction.Add(new Transaction { PlayerID = 10, LeavingTeamID = 1, ComingTeamID = 6 });
            context.SaveChanges();

            transactionsController = new TransactionsController(context);
        }

        [Fact]
        public void GetAllTransactions_ShouldReturnAllTransactions()
        {
            // Arrange

            // Act
            List<Transaction> transactions = transactionsController.GetTransaction().ToList();

            // Assert
            Assert.Equal(3, transactions.Count);
        }

        [Fact]
        public void GetTransactionById_ShouldReturnTheRightTransaction()
        {

            // Arrange
            int transactionId = context.Transaction.First<Transaction>().TransactionID;

            // Act
            var okResult = transactionsController.GetTransaction(transactionId).Result as OkObjectResult;

            // Assert
            Assert.IsType<Transaction>(okResult.Value);
            Assert.Equal(transactionId, (okResult.Value as Transaction).TransactionID);
        }

        [Fact]
        public void GetTransactionByWrongId_ShouldReturnNotFoundResult()
        {

            // Arrange
            int transactionId = 1000;

            // Act
            var notFoundResult = transactionsController.GetTransaction(transactionId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PutTransaction_ShouldAddNewItem()
        {

            // Arrange
            var transaction = context.Transaction.First<Transaction>();
            transaction.PlayerID = 1;

            // Act
            var noContentResult = transactionsController.PutTransaction(transaction.TransactionID, transaction);

            // Assert
            Assert.IsType<NoContentResult>(noContentResult.Result);
        }

        [Fact]
        public void PutTransactionDifferntIds_ShouldReturnBadRequest()
        {
            // Arrange
            int transactionId = 100100;
            var transaction = new Transaction()
            {
                TransactionID = 4,
                PlayerID = 12,
                LeavingTeamID = 17,
                ComingTeamID = 2
            };

            // Act
            var badRequestResult = transactionsController.PutTransaction(transactionId, transaction);

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult.Result);
        }

        [Fact]
        public void PutTransactionInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int transactionId = 100100;
            var transaction = new Transaction()
            {
                TransactionID = 100100,
                PlayerID = 12,
                LeavingTeamID = 17,
                ComingTeamID = 2
            };

            // Act
            var notFoundResult = transactionsController.PutTransaction(transactionId, transaction);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void PostTransaction_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var transaction = new Transaction()
            {
                PlayerID = 12,
                LeavingTeamID = 17,
                ComingTeamID = 2
            };

            // Act
            var createdResponse = transactionsController.PostTransaction(transaction);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
        }

        [Fact]
        public void PostTransactionInvalidData_ShouldReturnBadRequest()
        {
            // arrange
            var transaction = new Transaction()
            {
                PlayerID = 12,
                LeavingTeamID = 17
            };

            transactionsController.ModelState.AddModelError("ComingTeamID", "Required");

            // act
            var badresponse = transactionsController.PostTransaction(transaction);

            // assert
            Assert.IsType<BadRequestObjectResult>(badresponse.Result);
        }

        [Fact]
        public void DeleteTransaction_ShouldReturnOk()
        {
            // Arrange
            var transactionID = context.Transaction.First<Transaction>().TransactionID;

            // Act
            var okResponse = transactionsController.DeleteTransaction(transactionID);

            // Assert
            Assert.IsType<OkObjectResult>(okResponse.Result);
        }

        [Fact]
        public void DeleteTransactionInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var notExistingTransactionID = 100100;

            // Act
            var badResponse = transactionsController.DeleteTransaction(notExistingTransactionID);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse.Result);
        }
    }
}
