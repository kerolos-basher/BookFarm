using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using BookFarm.Entities.mail.Model;
using BookFarm.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly;

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
           //var BookFarms = await _context.books.ToListAsync();
           //var confirmationCodes = await _context.ConfirmBook.ToListAsync();
           //var places = await _context.places.ToListAsync();

      // Perform the left join
      //var leftJoinResult = from book in BookFarms
      //                     join code in confirmationCodes
      //                     on book.ConfirmCode equals code.ConfirmCode into codeGroup
      //                     from subCode in codeGroup.DefaultIfEmpty() // Left Join

      //                     select new
      //                     {
      //                       //just put all the data that you need here
      //                       BookId = book.Id,
      //                       BookName = book.Name??"",
      //                       BookEmail = book.Email??"",
      //                       ConfirmCode = book.ConfirmCode ?? "No Code",
      //                       confirmimage = subCode.PicturePath ?? "",
      //                     };


      var result = from book in _context.books
                   join confirmBook in _context.ConfirmBook
                   on book.ConfirmCode equals confirmBook.ConfirmCode into confirmGroup
                   from confirm in confirmGroup.DefaultIfEmpty() // LEFT JOIN
                   join place in _context.places
                   on book.placeID equals place.Id into placeGroup
                   from place in placeGroup.DefaultIfEmpty() // LEFT JOIN
                   select new
                   {
                     BookId = book.Id,
                     BookName = book.Name,
                     BookEmail=book.Email,
                     BookPhone = book.PhoneNumber,
                     BookDateFrom = book.DateFrom,
                     bookDateTo = book.DateTo,
                     bookImage = book.PicturePath,
                     ConfirmCode = book.ConfirmCode,
                     confirmImage = confirm.PicturePath,
                     placeId = place.Id,
                     placeName = place.Name,
                     placeDescription = place.Description,
                     placePriceForNight = place.PriceForNight,


                   };



      return Ok(result);
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
