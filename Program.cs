using HeavyGo_Project_Identity.Data;
using HeavyGo_Project_Identity.Hubs;
using HeavyGo_Project_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1?? Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSignalR();

// 2?? Add Identity (default template already uses Identity with ApplicationUser)
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // optional
}).AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 3?? Add MVC with views
builder.Services.AddControllersWithViews();

// 4?? Add Razor Pages (for Identity scaffold)
builder.Services.AddRazorPages();

var app = builder.Build();
// 5?? Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LandingPage}/{action=Index}/{id?}");

app.MapRazorPages(); // ? needed for Identity scaffolded pages
app.MapHub<LocationHub>("/locationHub");
app.Run();