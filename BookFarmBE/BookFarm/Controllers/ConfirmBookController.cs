using BookFarm.Controllers.Request;
using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using Microsoft.AspNetCore.Mvc;

namespace BookFarm.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ConfirmBookController : ControllerBase
  {
    private readonly DataContext _context;
    private readonly EmailService _emailService;

    public ConfirmBookController(DataContext context, EmailService emailService)
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

    [HttpPost("comfirmBook")]
    public IActionResult AddUser1([FromBody] confirmFarmRequest request)
    {
      if (request == null)
      {
        return BadRequest("Invalid data.");
      }



      // Save the relative path to the database (if needed)
      var relativePath = UploadImage(request.Picture, ".\\uploads");

      //send mail

      var confirmAFarm = new ConfirmBook
      {
        PicturePath = relativePath,
        ConfirmCode = request.ConfirmCode,
      };
      _context.ConfirmBook.Add(confirmAFarm);
      _context.SaveChanges();



      return Ok();

    }
  }
}
