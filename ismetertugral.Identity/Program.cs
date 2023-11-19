using ismetertugral.Identity.Context;
using ismetertugral.Identity.CustomDescriber;
using ismetertugral.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequiredLength = 1;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.SignIn.RequireConfirmedEmail = false;
    opt.Lockout.MaxFailedAccessAttempts = 3;
}).AddErrorDescriber<CustomErrorDescriber>().AddEntityFrameworkStores<IEContext>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true;
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    opt.ExpireTimeSpan = TimeSpan.FromDays(25);
    opt.Cookie.Name = "IsmetCookie";
    opt.LoginPath = new PathString("/Home/SignIn");
    opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

builder.Services.AddDbContext<IEContext>(opt =>
{
    opt.UseSqlServer("server=(localdb)\\MSSQLLocalDB; database=IdentityDb; integrated security=true; TrustServerCertificate=True;");
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/node_modules",
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules"))
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.Run();
