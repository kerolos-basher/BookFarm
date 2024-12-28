using BookFarm.Data;
using BookFarm.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DeleteBook : ControllerBase
    {
        private readonly DataContext _context;

        public DeleteBook(DataContext context)
        {
            _context = context;
        }
        [HttpGet("{ID}")]
        public async Task<ActionResult> DeleteBookID(int ID)
        {
            var bookE = _context.books.FirstOrDefault(e=>e.Id== ID);
            _context.books.Remove(bookE);
            _context.SaveChanges();

            return Ok();

        }


    }
}
