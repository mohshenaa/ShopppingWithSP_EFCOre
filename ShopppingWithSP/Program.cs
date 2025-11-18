using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Selfcare_meets_Beautify.Services;
using ShopppingWithSP.Models;

namespace ShopppingWithSP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ShoppingWithSP_DB>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("shop"));
            });

            builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ShoppingWithSP_DB>();

            ////customize idenity
            //builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ShoppingWithSP_DB>().AddDefaultTokenProviders();


            builder.Services.AddFileUploader();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
