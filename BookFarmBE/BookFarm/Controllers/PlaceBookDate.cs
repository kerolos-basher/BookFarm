using BookFarm.Controllers.Request;
using BookFarm.Data;
using BookFarm.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceBookDate : ControllerBase
    {
        private readonly DataContext _context;

        public PlaceBookDate(DataContext context)
        {
            _context = context;
        }
        [HttpGet("{ID}")]
        public async Task<ActionResult<List<BookAFarm>>> GetBookFarm(int ID)
        {
            var placesDates = await _context.books
             .Where(i => i.placeID == ID)
             .Select(i => new
             {
                 DateFrom = DateTime.Parse(i.DateFrom).Date.AddDays(-1),
                 DateTo = DateTime.Parse(i.DateTo).Date
             })
             .ToListAsync();

            var result = placesDates.SelectMany(dates =>
            {
                var fromDate = DateOnly.FromDateTime(dates.DateFrom);
                var toDate = DateOnly.FromDateTime(dates.DateTo);

                return Enumerable.Range(0, (toDate.ToDateTime(TimeOnly.MinValue) - fromDate.ToDateTime(TimeOnly.MinValue)).Days)
                    .Select(offset => fromDate.AddDays(offset));
            }).ToList();



            return Ok(result);



            var PlacesDates = await _context.books.Where(i => i.placeID == ID).ToListAsync();

            return Ok(PlacesDates);

        }


    }
}
