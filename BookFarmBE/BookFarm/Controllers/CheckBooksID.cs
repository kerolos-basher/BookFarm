using BookFarm.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckBooksID : ControllerBase
    {
        private readonly DataContext _context;

        public CheckBooksID(DataContext context)
        {
            _context = context;
        }
        public class BookRequest
        {
            public string Id { get; set; }
        }
        [HttpPost]

        public async Task<IActionResult> GetBookByID([FromBody] BookRequest request)
        {

            if (request == null || request.Id == "")
            {
                return BadRequest(new { message = "Invalid ID. Please provide a valid ID." });
            }

            var data = await _context.books.Where(p => p.ConfirmCode == request.Id.ToString()).ToListAsync(); // Assuming "Books" is your DbSet
            if (data.Count == 0 || data == null)
            {
                return NotFound(new { message = $"No book found with ID {request.Id}." });
            }

            return Ok(data);
        }
    }
}
