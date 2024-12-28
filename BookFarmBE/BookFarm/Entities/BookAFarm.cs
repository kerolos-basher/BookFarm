using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookFarm.Entities
{
    public class BookAFarm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Your ID is Required.")]
        public string PicturePath { get; set; } // Optional field for storing file path
        [NotMapped]
        public IFormFile PictureFile { get; set; }

        [Required(ErrorMessage = "DateFrom is required.")]
        public string DateFrom { get; set; }

        [Required(ErrorMessage = "DateTo is required.")]
        public string DateTo { get; set; }

      
        public string ConfirmCode { get; set; }

        public int? placeID { get; set; }


        //public string? ConfirmPicturePath { get; set; } // Optional field for storing file path
        //[NotMapped]
        //public IFormFile ConfirmPictureFile { get; set; }
    }   

}
