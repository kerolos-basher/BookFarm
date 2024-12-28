using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookFarm.Entities
{
    public class ConfirmBook
    {
        public int Id { get; set; }

        public string ConfirmCode { get; set; }

        [Required(ErrorMessage = "Your ID is Required.")]
        public string PicturePath { get; set; } // Optional field for storing file path
        [NotMapped]
        public IFormFile PictureFile { get; set; }

    }
}
