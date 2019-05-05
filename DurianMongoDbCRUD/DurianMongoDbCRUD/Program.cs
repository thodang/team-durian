using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DurianMongoDbCRUD.Model;
using DurianMongoDbCRUD.Repository;
using MongoDB.Bson;

namespace DurianMongoDbCRUD
{
    public class Program
    {
        /*
            1. populate the sample data (eg. by importing it from some CSV files).
            2. generate a list all the available books with the number of copies available for each.
            3. add a new customer.
            4. create a new order for this new customer with at least a couple of books.
            5. update the inventory to reflect the order being fulfilled.
         */
        private static string _exeDirectory;
        private const string NewCustomerName = "New Customer";
        private static string _newOrderId;
        private static string _newCustomerId;

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Durian Online Bookstore!\r\n");

            var process = true;
            _exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            while (process)
            {
                Console.WriteLine("Select one of the following options and press Enter: \r\n");
                Console.WriteLine("a - Populate Sample Data");
                Console.WriteLine("b - Generate list of all available books");
                Console.WriteLine("c - Add a new customer");
                Console.WriteLine("d - Create a new order for newly created customer");
                Console.WriteLine("e - Update the inventory for the checked out books");
                Console.WriteLine("x - Exit\r\n");
                Console.WriteLine("============================================================\r\n");


                var menu = Console.ReadLine();

                switch (menu)
                {
                    case "a":
                        Console.WriteLine("Clearing existing collections ...");
                        ClearAllCollections();
                        Console.WriteLine("Generating Books Collection ...");
                        PopulateBooksCollection();
                        Console.WriteLine("Generating Customers Collection ...");
                        PopulateCustomersCollection();
                        //Console.WriteLine("Generating Orders Collection ...");
                        //PopulateOrdersCollection();
                        break;

                    case "b":
                        Console.WriteLine("Generating list of all available books ...");
                        var books = GetAllBooks();
                        foreach (var book in books)
                        {
                            Console.WriteLine(book.ToString());
                        }
                        break;

                    case "c":
                        Console.WriteLine("Adding a new customer ...");
                        CreateCustomer().Wait();
                        break;

                    case "d":
                        Console.WriteLine("Creating order for new customer ...");
                        CreateOrderForCustomer().Wait();
                        break;

                    case "e":
                        Console.WriteLine("Updating inventory for newly created order ...");
                        //UpdateInventory();
                        break;

                    case "x":
                    default:
                        process = false;
                        break;
                }
            }
        }

        private static void ClearAllCollections()
        {
            try
            {
                var dbManager = new MongoDbManager();
                dbManager.ClearAllCollections();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void PopulateBooksCollection()
        {
            var filePath = $@"{_exeDirectory}\Seed\books.json";
            try
            {
                var dbManager = new MongoDbManager();
                dbManager.PopulateCollection(filePath, "books");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void PopulateCustomersCollection()
        {
            var filePath = $@"{_exeDirectory}\Seed\customers.json";
            try
            {
                var dbManager = new MongoDbManager();
                dbManager.PopulateCollection(filePath, "customers");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void PopulateOrdersCollection()
        {
            var filePath = $@"{_exeDirectory}\Seed\orders.json";
            try
            {
                var dbManager = new MongoDbManager();
                dbManager.PopulateCollection(filePath, "orders");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static List<Book> GetAllBooks()
        {
            try
            {
                var dbManager = new MongoDbManager();
                var books = dbManager.GetAllBooks();

                return books.Result;
            }
            catch (Exception e)
            {
                throw new Exception($"Exception thrown while getting list of books. {e.Message}");
            }
        }

        private static async Task CreateCustomer()
        {
            //_newCustomerId = Guid.NewGuid();
            var customer = new Customer
            {
                //customer_id = _newCustomerId,
                email = "newcustomer@gmail.com",
                name = NewCustomerName,
                address = new Address
                {
                    street = "1234 Freedom Circle",
                    city = "Santa Clara",
                    state = "CA",
                    zipcode = "95051"
                }
            };

            try
            {
                var dbManager = new MongoDbManager();
                var newCustomer = await dbManager.AddNewCustomer(customer);

                _newCustomerId = customer.Id;

                //var newCustomer = dbManager.GetCustomerById(_newCustomerId);
                Console.WriteLine($"Added customer : {newCustomer}");
            }
            catch (Exception e)
            {
                throw new Exception($"Exception thrown while creating a new customer. {e.Message}");
            }
        }
        
        private static async Task CreateOrderForCustomer()
        {
            //_newOrderId = Guid.NewGuid();

            var requestedBooksIsbn = new List<string>
            {
                "9781906523374",
                "9781592211517",
                "9780912469096"
            };

            var availableBooks = new List<Book>();

            try
            {
                var dbManager = new MongoDbManager();

                // Check if the requested books are available for checkout
                foreach (var isbn in requestedBooksIsbn)
                {
                    if (dbManager.IsInventoryAvailable(isbn))
                    {
                        var book = dbManager.GetBookByIsbn(isbn);
                        //add available inventory to new list
                        availableBooks.Add(book);
                    }
                    else
                    {
                        Console.WriteLine($"ISBN: {isbn} is not available for order");
                    }
                }

                var customer = dbManager.GetCustomerById(_newCustomerId);
                var availableBookIds = availableBooks.Select(id => id.Id).ToList();

                var order = new Order
                {
                    customer_id = customer.Id,
                    //order_id = _newOrderId,
                    book_id = availableBookIds
                };

                await dbManager.AddNewOrder(order);
                _newOrderId = order.Id;

            }
            catch (Exception e)
            {
                throw new Exception($"Exception thrown while creating an order for new customer. {e.Message}");
            }
        }
        /*
        private static void UpdateInventory()
        {
            try
            {
                var dbManager = new MongoDbManager();
                var newOrder = dbManager.GetOrderById(_newOrderId);
                var books = dbManager.GetAllBooks().Result;

                foreach (var b in books)
                {
                    if (!newOrder.book_id.Contains(b.book_id)) continue;
                    var availableInventory = b.available_inventory;
                    Console.WriteLine($"Old available Inventory {availableInventory}");
                    //Update inventory
                    b.available_inventory = availableInventory - 1;
                    dbManager.UpdateBookInventory(b);
                    Console.WriteLine($"New available Inventory {b.available_inventory}");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Exception thrown while updating book's available inventory. {e.Message}");
            }

        }
        */
    }
}
