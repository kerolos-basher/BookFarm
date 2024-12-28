using BookFarm.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookFarm.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            

        }
        public DbSet<BookAFarm> books { get; set; }
        public DbSet<Place> places { get; set; }
        public DbSet<CarouselItem> carouselItems { get; set; }
        public DbSet<ConfirmBook> ConfirmBook { get; set; }


    }
}
