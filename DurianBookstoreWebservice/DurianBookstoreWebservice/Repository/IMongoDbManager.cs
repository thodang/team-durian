using System.Collections.Generic;
using System.Threading.Tasks;
using DurianBookstoreWebservice.Model;

namespace DurianBookstoreWebservice.Repository
{
    public interface IMongoDbManager
    {
        /// <summary>
        /// Populate database with Seed data for Books, Customers and Orders collections
        /// </summary>
        /// <param name="jsonFilePath"></param>
        /// <param name="collectionName"></param>
        void PopulateCollection(string jsonFilePath, string collectionName);

        /// <summary>
        /// Clear all collections
        /// </summary>
        void ClearAllCollections();

        /// <summary>
        /// Check if a book has available inventory
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        bool IsInventoryAvailable(string bookId);

        /// <summary>
        /// Add a new document to Orders collection
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<string> AddNewOrder(Order order);

        /// <summary>
        /// Add a new document to Customers collection
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<string> AddNewCustomer(Customer customer);

        /// <summary>
        /// Get list of all the customers
        /// </summary>
        /// <returns></returns>
        Task<List<Customer>> GetAllCustomers();

        /// <summary>
        /// Get customer by CustomerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer GetCustomerById(string customerId);

        /// <summary>
        /// Get list of all the books
        /// </summary>
        /// <returns></returns>
        Task<List<Book>> GetAllBooks();

        /// <summary>
        /// Get list of available books - available_inventory > 0
        /// </summary>
        /// <returns></returns>
        Task<List<Book>> GetAvailableBooks();

        /// <summary>
        /// Get Book by BookId
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        Book GetBookById(string bookId);

        /// <summary>
        /// Get book by ISBN
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        Book GetBookByIsbn(string isbn);

        /// <summary>
        /// Get list of all the orders
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetAllOrders();

        /// <summary>
        /// Get Order by OrderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Order GetOrderById(string orderId);

        /// <summary>
        /// Go through books in an order and update available inventory 
        /// </summary>
        /// <param name="orderId"></param>
        bool OrderFulfilment(string orderId);

        /// <summary>
        /// Get all orders with is_fulfilled flag set to false
        /// </summary>
        /// <returns></returns>
        Task<List<Order>> GetOrdersForFulfillment();
    }
}
