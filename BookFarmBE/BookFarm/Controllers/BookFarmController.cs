using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using BookFarm.Entities.mail.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookFarmController : ControllerBase
    {
        private readonly DataContext _context;

        public BookFarmController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        

        public async Task<ActionResult<List<BookAFarm>>> GetALlBookFarm()
        {
           var BookFarms = await _context.books.ToListAsync();
            
        
            return Ok(BookFarms);
        }
        [HttpGet("{id}")]
       
        public async Task<ActionResult<List<BookAFarm>>> GetBookFarm(int id)
        {
            var BookFarms = await _context.books.FindAsync(id);
            if (BookFarms is null)
                return BadRequest("Book NotFound");

            
            return Ok(BookFarms);
        }



        

    }
}
