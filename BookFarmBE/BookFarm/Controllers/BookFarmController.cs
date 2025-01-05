using BookFarm.Data;
using BookFarm.Entities;
using BookFarm.Entities.mail;
using BookFarm.Entities.mail.Model;
using BookFarm.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System;

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
      #region 
      //     var BookFarms = await _context.books.ToListAsync();
      //     var confirmationCodes = await _context.ConfirmBook.ToListAsync();
      //     var places = await _context.places.ToListAsync();
      //// Perform the left join
      //var leftJoinResult = from book in BookFarms
      //                     join code in confirmationCodes
      //                     on book.ConfirmCode equals code.ConfirmCode into codeGroup
      //                     from subCode in codeGroup.DefaultIfEmpty() // Left Join
      //                     select new
      //                     {
      //                       //just put all the data that you need here
      //                       BookId = book.Id,
      //                       BookName = book.Name,
      //                       BookEmail = book.Email,
      //                       ConfirmCode = book.ConfirmCode ?? "No Code",
      //                       confirmimage = subCode.PicturePath ?? "",
      //                     };
      //var BookFarms = await _context.books.ToListAsync();
      //var confirmationCodes = await _context.ConfirmBook.ToListAsync();
      //var places = await _context.places.ToListAsync();
      //var leftJoinResult = from book in BookFarms
      //                     join code in confirmationCodes
      //                     on book.ConfirmCode equals code.ConfirmCode into codeGroup
      //                     from subCode in codeGroup.DefaultIfEmpty() // Left Join with confirmationCodes
      //                     join place in places
      //                     on book.placeID equals place.Id into placeGroup
      //                     from subPlace in placeGroup.DefaultIfEmpty() // Left Join with places
      //                     select new
      //                     {
      //                       // Add the data you need heyre
      //                       //BookId = book.Id,
      //                       //BookName = book.Name,
      //                       //BookEmail = book.Email,
      //                       //ConfirmCode = book.ConfirmCode ?? "No Code",
      //                       //ConfirmImage = subCode?.PicturePath ?? "",
      //                       //PlaceName = subPlace?.Name ?? "Unknown Place",
      //                       //PlaceAddress = subPlace?.Address ?? "No Address",

      //                          BookId = book.Id, // Corrected property names based on the provided JSON
      //                          BookName =  book.Name??"",
      //                          BookEmail = book.Email ?? "",
      //                          BookPhone = book.PhoneNumber ?? "",
      //                          BookFrom = book.DateFrom ?? "",
      //                          BookTo =  book.DateTo ?? "",
      //                          BookImg = book.PicturePath ?? "",
      //                          ConfirmCode =book.ConfirmCode ?? "",
      //                          Confirmimage = subCode.PicturePath??"",
      //                          PlaceID = subPlace.Id,
      //                          placeName = subPlace.Name ?? "",
      //                          placeDesc =subPlace.Description ??"",
      //                          placePrice =subPlace.PriceForNight,
      //                     };
      #endregion
      var BookFarms = await _context.books.ToListAsync();
      var confirmationCodes = await _context.ConfirmBook.ToListAsync();
      var places = await _context.places.ToListAsync();

      var leftJoinResult = from book in BookFarms
                           join code in confirmationCodes
                           on book.ConfirmCode equals code.ConfirmCode into codeGroup
                           from subCode in codeGroup.DefaultIfEmpty() // Left Join with confirmationCodes
                           join place in places
                           on book.placeID equals place.Id into placeGroup
                           from subPlace in placeGroup.DefaultIfEmpty() // Left Join with places
                           select new
                           {
                             BookId = book.Id,
                             BookName = book.Name ?? "",
                             BookEmail = book.Email ?? "",
                             BookPhone = book.PhoneNumber ?? "",
                             BookFrom = book.DateFrom ?? "", // Format dates as strings if needed
                             BookTo = book.DateTo ?? "",
                             BookImg = book.PicturePath ?? "",
                             ConfirmCode = book.ConfirmCode ?? "",
                             Confirmimage = subCode?.PicturePath ?? "", // Null-safe access
                             PlaceID = subPlace?.Id ?? 0, // Default to 0 if null
                             PlaceName = subPlace?.Name ?? "Unknown Place",
                             PlaceDesc = subPlace?.Description ?? "No Description",
                             PlacePrice = subPlace?.PriceForNight ?? 0 // Default to 0 if null
                           };

      return Ok(leftJoinResult);


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
