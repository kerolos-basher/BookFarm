using BookFarm.Controllers.Request;
using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Macs;
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
    [HttpPost("AddUser")]

    public async Task<IActionResult> AddUser([FromBody] BookFarmRequest request)
    {
      if (request == null)
      {
        return BadRequest("Invalid data.");
      }

      var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
      if (!Directory.Exists(uploadsFolder))
      {
        Directory.CreateDirectory(uploadsFolder);
      }

      // Generate a unique file name for the image
      var fileName = $"{Guid.NewGuid()}.jpg"; // Use .jpg or appropriate extension
      var filePath = Path.Combine(uploadsFolder, fileName);

      // Convert Base64 to byte array
      byte[] imageBytes = Convert.FromBase64String(request.Picture);

      // Write the image to the file system
      System.IO.File.WriteAllBytes(filePath, imageBytes);

      // Save the relative path to the database (if needed)
      var relativePath = $"/uploads/{fileName}";
      try
      {
        //email to the user
        var user_body = new StringBuilder();
        user_body.AppendLine("Your confirmation code is : " + request.ConfirmCode);
        user_body.AppendLine("الشروط والاحكام");
        user_body.AppendLine("ممنوع إصطحاب الحيوانات\r\nيتم دفع الرسوم مقدماً\r\nقيمة التأمين 1000 درهم \r\nاستيعاب الغرف للأشخاص \r\nالاستوديو ( 3 اشخاص) \r\nغرفة وصالة (5 اشخاص)\r\nغرفتين وصاله (7 اشخاص)");
        user_body.AppendLine("\n للمزيد من التفاصيل والتواصل");
        user_body.AppendLine("Phone : +971507155511");
        user_body.AppendLine("Email : Almarri.hassan@gmail.com");
        var r1 = await _emailService.SendEmailAsync(request.Email, "Villa Booking Details", user_body.ToString(), false);
      }
      catch (Exception ex)
      {
        return NotFound();
      }
      var bookAFarm = new BookAFarm
      {
        Name = request.Name,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber,
        PicturePath = relativePath,
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
      admin_body.AppendLine("phone number : "+ request.PhoneNumber);
      admin_body.AppendLine("Email : "+ request.Email);
      admin_body.AppendLine("Villa number : "+ request.PlaceID);
      admin_body.AppendLine("from date : "+ request.DateFrom);
      admin_body.AppendLine("to date : "+ request.DateTo);
      admin_body.AppendLine($"EID image link : https://api.liwavillas.com/{relativePath}");
      var r = await _emailService.SendEmailAsync("Almarri.hassan@gmail.com", "Villa Booking Details", admin_body.ToString(), false);

      return Ok();

    }

  }
}
