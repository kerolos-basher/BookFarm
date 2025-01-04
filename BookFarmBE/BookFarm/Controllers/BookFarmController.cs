using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using BookFarm.Entities.mail.Model;
using BookFarm.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookFarm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookFarmController : ControllerBase
    {
        private readonly DataContext _context;

        public BookFarmController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        

        public async Task<ActionResult<List<BookAFarm>>> GetALlBookFarm()
        {
           var BookFarms = await _context.books.ToListAsync();
           var confirmationCodes = await _context.ConfirmBook.ToListAsync();

      // Perform the left join
      var leftJoinResult = from book in BookFarms
                           join code in confirmationCodes
                           on book.ConfirmCode equals code.ConfirmCode into codeGroup
                           from subCode in codeGroup.DefaultIfEmpty() // Left Join
                           select new
                           {
                             //just put all the data that you need here
                             BookId = book.Id,
                             BookN  ame = book.Name,
                             BookEmail = book.Email,
                             ConfirmCode = book.ConfirmCode ?? "No Code",
                             confirmimage = subCode.PicturePath ?? "",
                           };



      return Ok(leftJoinResult);
        }
        [HttpGet("{id}")]
       
        public async Task<ActionResult<List<BookAFarm>>> GetBookFarm(int id)
        {
            var BookFarms = await _context.books.FindAsync(id);
            if (BookFarms is null)
                return BadRequest("Book NotFound");

            
            return Ok(BookFarms);
        }



        

    }
}
