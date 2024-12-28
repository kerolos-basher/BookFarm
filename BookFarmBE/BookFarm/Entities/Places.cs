using System.ComponentModel.DataAnnotations;

namespace BookFarm.Entities
{
    public class Place
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal PriceForNight { get; set; }
        public string imgUrl { get; set; }

        public ICollection<BookAFarm> bookAFarms { get; set; }
    }
}
