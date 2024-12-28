using BookFarm.Controllers.Request;
using BookFarm.Data;
using BookFarm.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmBookController : ControllerBase
    {
        private readonly DataContext _context;

        public ConfirmBookController(DataContext context)
        {
            _context = context;
        }


        [HttpPost("comfirmBook")]
        public IActionResult AddUser1([FromBody] confirmFarmRequest request)
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
