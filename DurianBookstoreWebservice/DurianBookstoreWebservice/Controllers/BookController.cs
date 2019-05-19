using System;
using System.Threading.Tasks;
using DurianBookstoreWebservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurianBookstoreWebservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IMongoDbManager _mongoDbManager;
        private readonly ILogger<BookController> _logger;

        public BookController(IMongoDbManager mongoDbManager, ILogger<BookController> logger)
        {
            _mongoDbManager = mongoDbManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _mongoDbManager.GetAllBooks();
                return Ok(JsonConvert.SerializeObject(books));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting list of books: {e.Message}");
            }

            return BadRequest("Error getting all books");
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAavailableBooks()
        {
            try
            {
                var books = await _mongoDbManager.GetAvailableBooks();
                return Ok(JsonConvert.SerializeObject(books));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting list of books: {e.Message}");
            }

            return BadRequest("Error getting all books");
        }

        [HttpGet("{isbn}")]
        public IActionResult GetBookByIsbn(string isbn)
        {
            try
            {
                var book = _mongoDbManager.GetBookByIsbn(isbn);
                if (book == null)
                {
                    return NotFound();
                }

                return Ok(JsonConvert.SerializeObject(book));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while getting book by ISBN: {e.Message}");
            }

            return BadRequest("Error getting book by ISBN");
        }
    }
}
