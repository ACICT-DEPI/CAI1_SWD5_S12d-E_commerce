using E_Commerce___DEPI.Session;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce___DEPI.Models
{
    public class DbIntities:DbContext
    {
        public DbIntities()
        {
            SessionHelper.SetDbContext(this);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FrameMat> FrameMats { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<OrderdItem> OrderdItems { get; set; }
        public DbSet<OrderArchive> OrderArchives { get; set; }
        public DbSet<UpholsteryMat> UpholsteryMats { get; set; }

        public DbSet<ShippmentCity> ShippmentCities { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer("Data Source=188.165.47.6,49778; Initial Catalog=GP_Database; User ID=depi; Password=123456qweQWE; TrustServerCertificate=True;");

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
