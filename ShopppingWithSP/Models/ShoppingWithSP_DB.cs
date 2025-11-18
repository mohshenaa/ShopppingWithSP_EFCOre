using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace ShopppingWithSP.Models
{
	public class ShoppingWithSP_DB : IdentityDbContext<AppUser, AppRole, Guid>
	{

        public DbSet<Order> Order { get; set; }
        public DbSet<Details> OrderDetails { get; set; }
        public ShoppingWithSP_DB(DbContextOptions options) : base(options)
        {

        }

    }


    //// when we customize identity
    // public class AppUser:IdentityUser<Guid>
    // {

    // }
    // public class AppRole : IdentityRole<Guid>
    // {

    // }

    // public class ShoppingWithSP_DB :IdentityDbContext<AppUser, AppRole, Guid>
    // {

    // }
}