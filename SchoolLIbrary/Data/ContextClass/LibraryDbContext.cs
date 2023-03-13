using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolLIbrary.Models;

namespace SchoolLIbrary.Data.ContextClass
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) 
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
                builder.Entity<IdentityRole>().HasData(
                    new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole { Name = "Student", NormalizedName = "STUDENT" },
                    new IdentityRole { Name = "Lecturer", NormalizedName = "LECTURER" }
                );
           
        }
        public DbSet<LibraryUser> LibraryUsers { get; set; }
        public DbSet<MaterialModel> Materials { get; set; }
        public DbSet<CheckoutModel> Checkouts { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }
        public DbSet<PurchaseModel> Purchases { get; set; }
        public DbSet<RegistrationCodeModel> Regcodes { get; set; }
    }
}
