using SchoolLIbrary.Data.ContextClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SchoolLIbrary.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using SchoolLIbrary.CustomTokenProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LibraryDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibrarySqlDb"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
        })
        .AddEntityFrameworkStores<LibraryDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders()
        .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailconfirmation"); 

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
     opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromDays(3));

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
