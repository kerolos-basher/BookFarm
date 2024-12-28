using BookFarm.Data;
using BookFarm.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly DataContext _context;

        public PlacesController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Place>>> GetALlPlaces()
        {
            var Places = await _context.places.ToListAsync();


            return Ok(Places);
        }
    }
}
