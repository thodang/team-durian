using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurianMongoDbCRUD.Model;

namespace DurianMongoDbCRUD.Repository
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
        /// <param name="isbn"></param>
        /// <returns></returns>
        bool IsInventoryAvailable(string isbn);

        /// <summary>
        /// Add a new document to Orders collection
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task AddNewOrder(Order order);

        /// <summary>
        /// Add a new document to Customers collection
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task AddNewCustomer(Customer customer);

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
        Customer GetCustomerById(Guid customerId);

        /// <summary>
        /// Get list of all the books
        /// </summary>
        /// <returns></returns>
        Task<List<Book>> GetAllBooks();

        /// <summary>
        /// Get Book by BookId
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        Book GetBookById(Guid bookId);

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
        Order GetOrderById(Guid orderId);

        void UpdateBookInventory(Book book);
    }
}