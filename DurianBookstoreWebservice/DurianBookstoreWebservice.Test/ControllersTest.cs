using System.Collections.Generic;
using System.Threading.Tasks;
using DurianBookstoreWebservice.Controllers;
using DurianBookstoreWebservice.Model;
using Moq;
using Xunit;
using DurianBookstoreWebservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DurianBookstoreWebservice.Test
{
    public class ControllersTest
    {
        [Fact]
        public async Task Test1_BookController_GetAllBooks()
        {            
            // Arrange
            var mockRepo = new Mock<IMongoDbManager>();
            var mockLogger = new Mock<ILogger<BookController>>();
           
            mockRepo.Setup(repo => repo.GetAllBooks()).ReturnsAsync(GetBooks());
            var bookController = new BookController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await bookController.GetAllBooks();

            // Assert            
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Test2_BookController_GetBookByIsbn_NotFound()
        {
            // Arrange
            const string isbn = "123456789";
            var mockRepo = new Mock<IMongoDbManager>();
            var mockLogger = new Mock<ILogger<BookController>>();

            mockRepo.Setup(repo => repo.GetBookByIsbn(isbn)).Returns((Book)null);
            var bookController = new BookController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = bookController.GetBookByIsbn(isbn);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Test3_OrderController_CreateNewOrder()
        {
            var order = new Order()
            {
                customer_id = "5cc7d8c521ac098870b001b5",
                book_id = new List<string>() { "5cc7d8c421ac098870b001a7", "5cc7d8c421ac098870b001a8" }
            };

            // Arrange
            var mockRepo = new Mock<IMongoDbManager>();
            var mockLogger = new Mock<ILogger<OrderController>>();

            mockRepo.Setup(repo => repo.AddNewOrder(order)).Returns((Task<string>) null);
            var orderController = new OrderController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = orderController.CreateNewOrder(new Order());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        private List<Book> GetBooks()
        {
            var books = new List<Book>
            {
                new Book()
                {
                    isbn = "123456789",
                    title = "Unit Test Book 1",
                    authors = new List<string>(){"John Smith", "Doug Wilson"},
                    available_inventory = 5,
                    total_inventory = 5,
                    price = 20.95
                },
                new Book()
                {
                    isbn = "987654321",
                    title = "Unit Test Book 1",
                    authors = new List<string>(){"Tom Sawyer"},
                    available_inventory = 5,
                    total_inventory = 5,
                    price = 30.95
                }
            };

            return books;
        }

        private List<Customer> GetCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer()
                {
                    name = "Customer 1",
                    address = new Address(){city = "Test City", state = "Test state", street = "123 Test street", zipcode = "98765"}
                }
            };

            return customers;
        }
    }
}
