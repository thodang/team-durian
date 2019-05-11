using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurianBookstoreWebservice.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DurianBookstoreWebservice.Repository
{
    public class MongoDbManager : IMongoDbManager
    {
        private readonly IMongoDatabase _database;
        private const string BooksCollection = "books";
        private const string CustomersCollection = "customers";
        private const string OrdersCollection = "orders";

        public MongoDbManager(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetValue<string>("MongoDb:ConnectionString"));
            _database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Database"));
        }

        public void PopulateCollection(string jsonFilePath, string collectionName)
        {
            var json = System.IO.File.ReadAllText(jsonFilePath);

            try
            {
                var doc = BsonSerializer.Deserialize<List<BsonDocument>>(json);
                var collection = _database.GetCollection<BsonDocument>(collectionName);

                collection.InsertManyAsync(doc).Wait();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ClearAllCollections()
        {
            try
            {
                _database.DropCollection(OrdersCollection);
                _database.DropCollection(CustomersCollection);
                _database.DropCollection(BooksCollection);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool IsInventoryAvailable(string bookId)
        {
            var book = GetBookById(bookId);

            if (book != null)
            {
                return book.available_inventory > 0;
            }

            return false;
        }

        public async Task<string> AddNewOrder(Order order)
        {
            try
            {
                order.is_fulfilled = false;
                var collection = _database.GetCollection<Order>(OrdersCollection);
                await collection.InsertOneAsync(order);

                return order.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> AddNewCustomer(Customer customer)
        {
            var customers = _database.GetCollection<Customer>(CustomersCollection);

            try
            {
                await customers.InsertOneAsync(customer);
                var objectId = customer.Id;

                return objectId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Customer GetCustomerById(string customerId)
        {
            var customers = _database.GetCollection<Customer>(CustomersCollection);
            var filter = new FilterDefinitionBuilder<Customer>().Eq("Id", customerId);

            try
            {
                var result = customers.Find(filter).FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var customers = _database.GetCollection<Customer>(CustomersCollection);
            var filter = new FilterDefinitionBuilder<Customer>().Empty;
            try
            {
                var results = await customers.Find(filter).ToListAsync();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Book>> GetAllBooks()
        {
            var books = _database.GetCollection<Book>(BooksCollection);
            var filter = new FilterDefinitionBuilder<Book>().Empty;
            try
            {
                var results = await books.Find(filter).ToListAsync();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Book>> GetAvailableBooks()
        {
            var books = _database.GetCollection<Book>(BooksCollection);
            var filter = new FilterDefinitionBuilder<Book>().Gt(b => b.available_inventory, 0);
            try
            {
                var results = await books.Find(filter).ToListAsync();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Book GetBookById(string bookId)
        {
            var books = _database.GetCollection<Book>(BooksCollection);
            var filter = new FilterDefinitionBuilder<Book>().Eq("Id", ObjectId.Parse(bookId));

            try
            {
                var result = books.Find(filter).FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Book GetBookByIsbn(string isbn)
        {
            var books = _database.GetCollection<Book>(BooksCollection);
            var filter = new FilterDefinitionBuilder<Book>().Eq(b => b.isbn, isbn);

            try
            {
                var result = books.Find(filter).FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = _database.GetCollection<Order>(OrdersCollection);
            var filter = new FilterDefinitionBuilder<Order>().Empty;
            try
            {
                var results = await orders.Find(filter).ToListAsync();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Order GetOrderById(string orderId)
        {
            var orders = _database.GetCollection<Order>(OrdersCollection);
            var filter = new FilterDefinitionBuilder<Order>().Eq("Id", ObjectId.Parse(orderId));

            try
            {
                var result = orders.Find(filter).FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Order>> GetOrdersForFulfillment()
        {
            var orders = _database.GetCollection<Order>(OrdersCollection);
            var filter = new FilterDefinitionBuilder<Order>().Eq("is_fulfilled", false);

            try
            {
                var results = await orders.Find(filter).ToListAsync();
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool OrderFulfilment(string orderId)
        {
            var orders = _database.GetCollection<Order>(OrdersCollection);
            var books = _database.GetCollection<Book>(BooksCollection);
            var filter = new FilterDefinitionBuilder<Order>().Eq("Id", ObjectId.Parse(orderId));

            try
            {
                var order = orders.Find(filter).FirstOrDefault();
                foreach (var bookId in order.book_id)
                {
                    var book = GetBookById(bookId);
                    var availableInventory = book.available_inventory - 1;
                    if (availableInventory < 0)
                        return false;
                    var bookUdateFilter = Builders<Book>.Filter.Eq("Id", ObjectId.Parse(book.Id));
                    var updateBook = Builders<Book>.Update.Set(x => x.available_inventory, availableInventory);
                    var bookUpdateResult = books.UpdateOneAsync(bookUdateFilter, updateBook).Result;
                }

                //Set order.is_fulfilled property to true and update order collection
                var orderUpdateFilter = Builders<Order>.Filter.Eq("Id", ObjectId.Parse(orderId));
                var updateOrder = Builders<Order>.Update.Set(x => x.is_fulfilled, true);
                var orderUpdateResult = orders.UpdateOneAsync(orderUpdateFilter, updateOrder).Result;

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
