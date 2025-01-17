using BookFarm.Controllers.Request;
using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using BookFarm.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using System.Text;

namespace BookFarm.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BookPostController : ControllerBase
  {
    private readonly DataContext _context;
    private readonly EmailService _emailService;

    public BookPostController(DataContext context, EmailService emailService)
    {
      _context = context;
      _emailService = emailService;
    }
    private string UploadImage(string base64Image, string uploadDirectory)
    {
      // Ensure the directory exists
      if (!Directory.Exists(uploadDirectory))
      {
        Directory.CreateDirectory(uploadDirectory);
      }

      // Decode the Base64 string into a byte array
      byte[] imageBytes = Convert.FromBase64String(base64Image);

      // Determine the file extension dynamically
      string extension = GetImageExtension(imageBytes);

      if (string.IsNullOrEmpty(extension))
      {
        throw new Exception("Unsupported image format.");
      }

      // Generate a unique file name using a GUID
      string fileName = $"{Guid.NewGuid()}.{extension}";
      string fullPath = Path.Combine(uploadDirectory, fileName);
      string returnPath = Path.Combine(uploadDirectory.Replace(".", ""), fileName);

      // Write the file to the directory
      System.IO.File.WriteAllBytes(fullPath, imageBytes);

      // Return the full path of the uploaded image
      return returnPath;
    }

    private string GetImageExtension(byte[] fileBytes)
    {
      // Convert the first few bytes to a magic number (hex string)
      string magicNumber = BitConverter.ToString(fileBytes.Take(4).ToArray());

      // Dictionary of magic numbers and file extensions
      var magicNumberDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "FF-D8-FF", "jpg" },     // JPEG
            { "89-50-4E-47", "png" },  // PNG
            { "47-49-46-38", "gif" },  // GIF
            { "42-4D", "bmp" },        // BMP
            { "49-49-2A-00", "tiff" }, // TIFF
            { "4D-4D-00-2A", "tiff" }, // TIFF
        };

      // Check if the magic number matches any known file types
      foreach (var kvp in magicNumberDictionary)
      {
        if (magicNumber.StartsWith(kvp.Key))
        {
          return kvp.Value;
        }
      }

      return null; // Unsupported file type
    }

    [HttpPost("AddUser")]

    public async Task<IActionResult> AddUser([FromBody] BookFarmRequest request)
    {

      //var BookFarms = await _context.books.Where(b => b.DateFrom).ToListAsync();
      //var confirmationCodes = await _context.ConfirmBook.ToListAsync();


      if (request == null)
      {
        return BadRequest("Invalid data.");
      }
      try
      {

        // Parse input dates
        // Define multiple date formats to handle inconsistencies
        string[] dateFormats = new[] {
              "yyyy-M-dTHH:mm:ss.fffZ",  // Single-digit month/day with milliseconds
              "yyyy-MM-ddTHH:mm:ss.fffZ", // Double-digit month/day with milliseconds
              "yyyy-M-dTHH:mm:ssZ",      // Single-digit month/day without milliseconds
              "yyyy-M-ddTHH:mm:ssZ",      // Single-digit month/day without milliseconds
              "yyyy-MM-dTHH:mm:ssZ",      // Single-digit month/day without milliseconds
              "yyyy-MM-ddTHH:mm:ssZ",    // Double-digit month/day without milliseconds
              "yyyy-M-d",                // Single-digit month/day, no time
              "yyyy-MM-dd"               // Double-digit month/day, no time
          };

        var culture = CultureInfo.InvariantCulture;

        if (!DateTime.TryParseExact(request.DateFrom?.Trim(), dateFormats, culture, DateTimeStyles.AssumeUniversal, out DateTime parsedFromDate) ||
            !DateTime.TryParseExact(request.DateTo?.Trim(), dateFormats, culture, DateTimeStyles.AssumeUniversal, out DateTime parsedToDate))
        {
          return BadRequest("Invalid date format. Expected format: yyyy-MM-ddTHH:mm:ss.fffZ.");
        }

        // Retrieve and parse data from the database
        var BookFarms = await _context.books.ToListAsync();
        var confirmationCodes = await _context.ConfirmBook.ToListAsync();
        var Places = await _context.places.ToListAsync();

        // Perform the inner join
        var innerJoinResult = (from book in BookFarms
                               join code in confirmationCodes
                               on book.ConfirmCode equals code.ConfirmCode
                               select new
                               {
                                 BookId = book.Id
                               }).ToList();

        // Filter BookFarms based on the inner join result
        var filteredBookFarms = BookFarms.Where(b => innerJoinResult.Any(r => r.BookId == b.Id)).ToList();

        
        foreach (var booking in filteredBookFarms)
        {
          // Check if the PlaceID matches
          if (booking.placeID.ToString() == request.PlaceID)
          {
            // Attempt to parse the dates
            if (DateTime.TryParseExact(booking.DateFrom?.Trim(), dateFormats, culture, DateTimeStyles.AssumeUniversal, out DateTime dbStartDate) &&
                DateTime.TryParseExact(booking.DateTo?.Trim(), dateFormats, culture, DateTimeStyles.AssumeUniversal, out DateTime dbEndDate))
            {
              // Evaluate conflict conditions
              if (
                  (dbStartDate <= parsedFromDate && dbEndDate >= parsedFromDate) || // New booking starts in existing booking
                  (dbStartDate <= parsedToDate && dbEndDate >= parsedToDate) ||   // New booking ends in existing booking
                  (dbStartDate >= parsedFromDate && dbEndDate <= parsedToDate) || // New booking fully contains existing booking
                  (dbStartDate <= parsedFromDate && dbEndDate >= parsedToDate)    // Existing booking fully contains new booking
              )
              {
                return NotFound($"Conflict detected: Booking ID {booking.Id} conflicts with the new booking.");
              }
            }
            else
            {
              // Log invalid date format for debugging
              Console.WriteLine($"Failed to parse dates for Booking ID: {booking.Id}, DateFrom: {booking.DateFrom}, DateTo: {booking.DateTo}");
            }
          }
        }


        





        //if ()
        //{

        //}
        var path = UploadImage(request.Picture, ".\\uploads");
        //email to the user
        var user_body = new StringBuilder();
        user_body.AppendLine("Your confirmation code is : " + request.ConfirmCode);
        user_body.AppendLine("الشروط والاحكام");
        user_body.AppendLine("ممنوع إصطحاب الحيوانات\r\nيتم دفع الرسوم مقدماً\r\nقيمة التأمين 1000 درهم \r\nاستيعاب الغرف للأشخاص \r\nالاستوديو ( 3 اشخاص) \r\nغرفة وصالة (5 اشخاص)\r\nغرفتين وصاله (7 اشخاص)");
        user_body.AppendLine("\n للمزيد من التفاصيل والتواصل");
        user_body.AppendLine("Phone : +971507155511");
        user_body.AppendLine("Email : Almarri.hassan@gmail.com");
        var r1 = await _emailService.SendEmailAsync(request.Email, "Villa Booking Details", user_body.ToString(), false);

        var bookAFarm = new BookAFarm
        {
          Name = request.Name,
          Email = request.Email,
          PhoneNumber = request.PhoneNumber,
          PicturePath = path,
          DateFrom = request.DateFrom,
          DateTo = request.DateTo,
          ConfirmCode = request.ConfirmCode,
          placeID = string.IsNullOrEmpty(request.PlaceID) ? (int?)null : int.Parse(request.PlaceID)
        };
        _context.books.Add(bookAFarm);
        _context.SaveChanges();

        //email to the admin
        var admin_body = new StringBuilder();
        admin_body.AppendLine("book with confirmation code : " + request.ConfirmCode);
        admin_body.AppendLine("for : " + request.Name);
        admin_body.AppendLine("phone number : " + request.PhoneNumber);
        admin_body.AppendLine("Email : " + request.Email);
        admin_body.AppendLine("Villa number : " + request.PlaceID);
        admin_body.AppendLine("from date : " + request.DateFrom);
        admin_body.AppendLine("to date : " + request.DateTo);
        admin_body.AppendLine($"EID image link : https://api.liwavillas.com{path}");
        var r = await _emailService.SendEmailAsync("Almarri.hassan@gmail.com", "Villa Booking Details", admin_body.ToString(), false);
      }
      catch (Exception)
      {
        return NotFound();
      }
      return Ok();

    }
    [HttpGet()]

    public async Task<IActionResult> AddUser1()
    {
      var result = await _emailService.SendEmailAsync(
      email: "kerolos0onsy@gmail.com",
      subject: "Test Email",
      body: "<h1>Hello World!</h1>",
      isHtml: true
      );

      if (result.IsSuccess)
      {
        Console.WriteLine("Email sent successfully!");
      }
      else
      {
        Console.WriteLine($"Failed to send email: {result.Message}");
      }

      return Ok();

    }

  }
}
