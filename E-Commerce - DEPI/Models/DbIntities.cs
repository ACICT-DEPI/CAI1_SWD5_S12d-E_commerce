using Microsoft.EntityFrameworkCore;

namespace E_Commerce___DEPI.Models
{
    public class DbIntities:DbContext
    {
        public DbIntities()
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<FrameMat> FrameMats { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<OrderdItem> OrderdItems { get; set; }
        public DbSet<OrderedItemsArchive> OrderedItemsArchives { get; set; }
        public DbSet<OrdersArchive> OrdersArchives { get; set; }
        public DbSet<UpholsteryMat> UpholsteryMats { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer("Data Source=.\\SQLSERVER;Initial Catalog=GP_Database;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }

    }
}
