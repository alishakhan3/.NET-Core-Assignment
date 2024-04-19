using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Data;
using WebApplication1.Service;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddDefaultIdentity<WebApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<WebDbContext>();

builder.Services.AddTransient<UserService>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
