using BookFarm.Migrations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookFarm.Entities
{
  public class Place
  {
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public decimal PriceForNight { get; set; }
    public string imgUrl { get; set; }

    public ICollection<BookAFarm> bookAFarms { get; set; }


    public string GetTotalPrice(int PlaceID, string FromDate, string ToDate,decimal PFN)
    {
      FromDate =DateTime.Parse(FromDate).ToString("dd/MM/yyyy");
      ToDate = DateTime.Parse(ToDate).ToString("dd/MM/yyyy");

      try
      {
        // Define the expected date format (DD/MM/YYYY)
        string dateFormat = "dd/MM/yyyy";
        var culture = CultureInfo.InvariantCulture;

        // Parse the dates using the specified format
        if (!DateTime.TryParseExact(FromDate, dateFormat, culture, DateTimeStyles.None, out DateTime startDate))
        {
          return ("Invalid FromDate format. Expected format: DD/MM/YYYY.");
        }

        if (!DateTime.TryParseExact(ToDate, dateFormat, culture, DateTimeStyles.None, out DateTime endDate))
        {
          return ("Invalid ToDate format. Expected format: DD/MM/YYYY.");
        }

        // Ensure the start date is before the end date
        if (endDate < startDate)
        {
          return("EndDate must be greater than or equal to StartDate.");
        }

        // Retrieve the place from the database
       
        // Convert the daily price
        double dailyPrice = Convert.ToDouble(PFN);

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

        return re;
       
      }
      catch (Exception ex)
      {
        // Handle unexpected errors
        return "Bad Request";
      }

    }
  }
  }
