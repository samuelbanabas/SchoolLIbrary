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
        public DbSet<LibraryUser> LibraryUsers { get; set; }
        public DbSet<MaterialModel> Materials { get; set; }
        public DbSet<CheckoutModel> Checkouts { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }
        public DbSet<PurchaseModel> Purchases { get; set; }
    }
}
