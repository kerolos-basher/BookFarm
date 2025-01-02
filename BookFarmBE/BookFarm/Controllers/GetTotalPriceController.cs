using BookFarm.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
    public IActionResult TotalPrice(int PlaceID, string FromDate, string ToDate)
    {
      try
      {
        // Define the expected date format (DD/MM/YYYY)
        string dateFormat = "dd/MM/yyyy";
        var culture = CultureInfo.InvariantCulture;

        // Parse the dates using the specified format
        if (!DateTime.TryParseExact(FromDate, dateFormat, culture, DateTimeStyles.None, out DateTime startDate))
        {
          return BadRequest("Invalid FromDate format. Expected format: DD/MM/YYYY.");
        }

        if (!DateTime.TryParseExact(ToDate, dateFormat, culture, DateTimeStyles.None, out DateTime endDate))
        {
          return BadRequest("Invalid ToDate format. Expected format: DD/MM/YYYY.");
        }

        // Ensure the start date is before the end date
        if (endDate < startDate)
        {
          return BadRequest("EndDate must be greater than or equal to StartDate.");
        }

        // Retrieve the place from the database
        var place = _context.places.FirstOrDefault(p => p.Id == PlaceID);
        if (place == null)
        {
          return NotFound("Place not found.");
        }

        // Convert the daily price
        double dailyPrice = Convert.ToDouble(place.PriceForNight);

        // Define the special days
        var specialDays = new List<DateTime>
        {
            new DateTime(2024, 12, 30),
            new DateTime(2024, 12, 31),
            new DateTime(2025, 1, 1),
            new DateTime(2025, 1, 2),
            new DateTime(2025, 1, 3),
            new DateTime(2025, 1, 4),
            new DateTime(2025, 1, 5),
        };

        // Calculate total cost
        int totalDays = (endDate - startDate).Days + 1;
        double totalCost = 0;

        for (int i = 0; i < totalDays; i++)
        {
          DateTime currentDate = startDate.AddDays(i);

          // Check if the current date is a special day
          if (specialDays.Contains(currentDate.Date))
          {
            totalCost += dailyPrice * 2; // Double the price for special days
          }
          else
          {
            totalCost += dailyPrice;
          }
        }
        var re = $"{totalCost.ToString()} AED for {totalDays} Days";
        // Return the total cost
      
        return Ok(new
        {
          price = re
        });
      }
      catch (Exception ex)
      {
        // Handle unexpected errors
        return StatusCode(500, $"An error occurred: {ex.Message}");
      }
    }



  }




}
