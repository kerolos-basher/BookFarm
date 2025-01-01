using BookFarm.Controllers.Request;
using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using Microsoft.AspNetCore.Mvc;
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
      string returnPath = Path.Combine(uploadDirectory.Replace(".",""), fileName);

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
      if (request == null)
      {
        return BadRequest("Invalid data.");
      }
      try
      {
        
        var path = UploadImage(request.Picture,".\\uploads");
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
      catch (Exception ex)
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
