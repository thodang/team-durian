using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DurianMongoDbCRUD.Model;
using DurianMongoDbCRUD.Repository;
using Xunit;
using Newtonsoft.Json;

namespace DurianMongoDbCRUD.Test
{
    /*
       1. populate the sample data (eg. by importing it from some CSV files).
       2. generate a list all the available books with the number of copies available for each.
       3. add a new customer.
       4. create a new order for this new customer with at least a couple of books.
       5. update the inventory to reflect the order being fulfilled.
     */
    [Collection("Sequential")]
    public class UnitTestDbManager
    {
        private static readonly string ExeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private readonly MongoDbManager _dbManager;
        private string _newCustomerId;
        private string _newOrderId;
        private readonly List<string> _requestedBooksIsbn = new List<string>
        {
            "9781906523374",
            "9781592211517",
            "9780912469096"
        };


    public UnitTestDbManager()
        {
            _dbManager = new MongoDbManager();
        }

        [Fact]
        public void Test1_PopulateCollections()
        {
            _dbManager.ClearAllCollections();

            // Populate Books Collection
            var filePath = $@"{ExeDirectory}\Seed\books.json";
            _dbManager.PopulateCollection(filePath, "books");

            var books = _dbManager.GetAllBooks().Result;
            Assert.Equal(15, books.Count);

            // Populate Customers Collection
            filePath = $@"{ExeDirectory}\Seed\customers.json";
            _dbManager.PopulateCollection(filePath, "customers");

            var customers = _dbManager.GetAllCustomers().Result;
            Assert.Equal(15, customers.Count);

            // Populate Orders Collection
            filePath = $@"{ExeDirectory}\Seed\orders.json";
            _dbManager.PopulateCollection(filePath, "orders");

            var orders = _dbManager.GetAllOrders().Result;
            Assert.Equal(10, orders.Count);
        }

        [Fact]
        public void Test2_GenerateBooksList()
        {
            var books = _dbManager.GetAllBooks().Result;
            Assert.Equal(15, books.Count);
        }

        [Fact]
        public void Test3_AddNewCustomer()
        {
            var dbManager = new MongoDbManager();
            var newCustomer = NewCustomer();
            _newCustomerId = dbManager.AddNewCustomer(newCustomer).Result;

            var dbCustomer = dbManager.GetCustomerById(_newCustomerId);

            var dbCustomerStr = JsonConvert.SerializeObject(dbCustomer);
            var customerStr = JsonConvert.SerializeObject(newCustomer);
            Assert.Equal(dbCustomerStr, customerStr);
        }

        [Fact]
        public void Test4_CreateNewCustomerOrder()
        {
            var availableBooks = new List<Book>();
            var dbManager = new MongoDbManager();

            dbManager.AddNewCustomer(NewCustomer()).Wait();

            // Check if the requested books are available for checkout
            foreach (var isbn in _requestedBooksIsbn)
            {
                if (!dbManager.IsInventoryAvailable(isbn)) continue;
                var book = dbManager.GetBookByIsbn(isbn);
                //add available inventory to new list
                availableBooks.Add(book);
            }

            var availableBookIds = availableBooks.Select(id => id.Id).ToList();

            var order = new Order
            {
                customer_id = NewCustomer().Id,
                book_id = availableBookIds
            };

            _newOrderId = dbManager.AddNewOrder(order).Result;

            var dbOrder = dbManager.GetOrderById(_newOrderId);

            var dbOrderStr = JsonConvert.SerializeObject(dbOrder);
            var orderStr = JsonConvert.SerializeObject(order);
            Assert.Equal(dbOrderStr, orderStr);
        }

        [Fact]
        public void Test5_UpdateInventoryForNewOrder()
        {
            var availableBooks = new List<Book>();
            var dbManager = new MongoDbManager();

            dbManager.AddNewCustomer(NewCustomer()).Wait();

            // Check if the requested books are available for checkout
            foreach (var isbn in _requestedBooksIsbn)
            {
                if (!dbManager.IsInventoryAvailable(isbn)) continue;
                var book = dbManager.GetBookByIsbn(isbn);
                //add available inventory to new list
                availableBooks.Add(book);
            }

            var availableBookIds = availableBooks.Select(id => id.Id).ToList();

            var order = new Order
            {
                customer_id = NewCustomer().Id,
                book_id = availableBookIds
            };

            _newOrderId = dbManager.AddNewOrder(order).Result;

            var dbOrder = dbManager.GetOrderById(_newOrderId);

            var books = dbManager.GetAllBooks().Result;

            foreach (var b in books)
            {
                if (!dbOrder.book_id.Contains(b.Id)) continue;
                var availableInventory = b.available_inventory;
                b.available_inventory = availableInventory - 1;
                dbManager.UpdateBookInventory(b);
                Assert.Equal(availableInventory -1, b.available_inventory);
            }
        }


        private Customer NewCustomer()
        {
            return new Customer
            {
                email = "newcustomer@gmail.com",
                name = "New Customer",
                address = new Address
                {
                    street = "1234 Freedom Circle",
                    city = "Santa Clara",
                    state = "CA",
                    zipcode = "95051"
                }
            };
        }
    }
}
