using Bogus;
using Microsoft.EntityFrameworkCore;

namespace GP.Models
{
    public class DbIntities:DbContext
    {
        public DbIntities()
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<FrameMat> FrameMats { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=GP_Database;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //To Creat A Composite Key using fluent Api 
            modelBuilder.Entity<CartItem>()
                .HasKey(b => new { b.CustomerID, b.ProductId });
            modelBuilder.Entity<Feedback>()
                .HasKey(b => new { b.CustomerID, b.ProductId });
            base.OnModelCreating(modelBuilder);

            // Generate Random data for Customer's Table
            
            #region Generate Random data for Customer's Table
            //var customerFaker = new Faker<Customer>()
            //    .RuleFor(c => c.FullName, f => f.Company.CompanyName())
            //    ;
            //var Customers = customerFaker.Generate(1000); 
            #endregion

        }

    }
}
