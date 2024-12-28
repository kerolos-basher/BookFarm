using BookFarm.Data;
using BookFarm.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
   
    public class CarouselController : ControllerBase
    {
        private readonly DataContext _context;

        public CarouselController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("carousel")]
        public async Task<ActionResult<List<CarouselItem>>> GetCarouselItems()
        {

            var items = await _context.carouselItems.ToListAsync(); // Fetch from your database
            return Ok(items);
        }
       
    }

}
