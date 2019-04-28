using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurianMongoDbCRUD.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace DurianMongoDbCRUD.Repository
{
    public class MongoDbManager : IMongoDbManager
    {
        private readonly IMongoDatabase _database;
        private const string ConnectionString = "mongodb+srv://durian:cmpe272durian@online-bookstore-durian-cv8v3.azure.mongodb.net/test?retryWrites=true";
        private const string DatabaseName = "durian_bookstore_db";
        private const string BooksCollection = "books";
        private const string CustomersCollection = "customers";
        private const string OrdersCollection = "orders";

        public MongoDbManager()
        {
            var mongoClient = new MongoClient(ConnectionString);
            _database = mongoClient.GetDatabase(DatabaseName);
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
                Console.WriteLine(e);
                throw;
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
                Console.WriteLine(e);
                throw;
            }
        }

        public bool IsInventoryAvailable(string isbn)
        {
            var book = GetBookByIsbn(isbn);
            //var allBooks = GetAllBooks().Result;
            //var selectedBook = book.FirstOrDefault(a => a.book_id == bookId);

            if (book != null)
            {
                return book.available_inventory > 0;
            }

            return false;
        }

        public async Task AddNewOrder(Order order)
        {
            try
            {
                var collection = _database.GetCollection<Order>(OrdersCollection);
                await collection.InsertOneAsync(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AddNewCustomer(Customer customer)
        {
            try
            {
                var collection = _database.GetCollection<Customer>(CustomersCollection);
                await collection.InsertOneAsync(customer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Customer GetCustomerById(Guid customerId)
        {
            var customers = _database.GetCollection<Customer>(CustomersCollection);
            var filter = new FilterDefinitionBuilder<Customer>().Eq("customer_id", customerId);

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

        public Book GetBookById(Guid bookId)
        {
            var books = _database.GetCollection<Book>(BooksCollection);
            var filter = new FilterDefinitionBuilder<Book>().Eq(b=> b.book_id, bookId);

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

        public Order GetOrderById(Guid orderId)
        {
            var orders = _database.GetCollection<Order>(OrdersCollection);
            var filter = new FilterDefinitionBuilder<Order>().Eq("order_id", orderId);

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

        public void UpdateBookInventory(Book book)
        {
            try
            {
                var books = _database.GetCollection<Book>(BooksCollection);

                var filter = Builders<Book>.Filter.Where(x => x.isbn == book.isbn);
                var update = Builders<Book>.Update.Set(x => x.available_inventory, book.available_inventory);
                var result = books.UpdateOneAsync(filter, update).Result;              
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }           
        }
    }
}
