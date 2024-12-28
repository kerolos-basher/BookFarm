using BookFarm.Controllers.Request;
using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using BookFarm.Entities.mail.Model;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookFarm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookPostController : ControllerBase
    {
        private readonly DataContext _context;

        public BookPostController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("AddUser")]

        public IActionResult AddUser([FromBody] BookFarmRequest request)
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

            //send mail

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
             
            

            return Ok();

        }
        
    }
}
