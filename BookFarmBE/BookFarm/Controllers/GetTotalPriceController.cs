using BookFarm.Data;
using BookFarm.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookFarm.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GetTotalPriceController : ControllerBase
  {
    private readonly DataContext _context;

    public GetTotalPriceController(DataContext context)
    {
      _context = context;
    }
    [HttpGet("TotalPrice")]
    public async Task<IActionResult> TotalPrice(int PlaceID,string FromDate,string ToDate)
    {
     //double price = 0;
     //var dates = _context.priceForSpacialDates.Where(p=>p.placeID==PlaceID);
     //decimal i = _context.places.Where(p => p.Id == PlaceID).Select(p=>p.PriceForNight).FirstOrDefault();
  

      //DateTime startDate = DateTime.Parse(FromDate);
      //DateTime endDate = DateTime.Parse(ToDate);// End date


      //for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
      //{
      //  foreach (var item in dates)
      //  {
      //    if (item.Date.CompareTo(date.ToString())==0)
      //    {
      //      price += item.price;
      //    }
      //    else
      //    {
      //      price +=(double) i;
      //    }
      //  }
      //}

      return Ok('5');

    // return Ok(price);



     
    }


  }




}
