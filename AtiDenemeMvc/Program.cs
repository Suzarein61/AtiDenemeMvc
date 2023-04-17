using Business.Abstract;
using Business.Concrete;
using Core;
using DataAccess.Abstract;
using DataAccess.Concrate.EntityFramework;
using DataAccess.Concrate.EntityFramework.Context;
using Entities.Concrete.Identity;
using ExceptionHandling;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using System;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IProductDal, EfProductDal>();
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategorySerivce, CategoryManager>();

builder.Services.AddDbContext<DenemeContext>();


builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequiredLength = 4; //En az ka� karakterli olmas� gerekti�ini belirtiyoruz.
    x.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunlulu�unu kald�r�yoruz.
    x.Password.RequireLowercase = false; //K���k harf zorunlulu�unu kald�r�yoruz.
    x.Password.RequireUppercase = false; //B�y�k harf zorunlulu�unu kald�r�yoruz.
    x.Password.RequireDigit = false; //0-9 aras� say�sal karakter zorunlulu�unu kald�r�yoruz.

    x.User.RequireUniqueEmail = true; //Email adreslerini tekille�tiriyoruz.
    x.User.AllowedUserNameCharacters = "abc�defghi�jklmno�pqrs�tu�vwxyzABC�DEFGHI�JKLMNO�PQRS�TU�VWXYZ0123456789-._@+"; //Kullan�c� ad�nda ge�erli olan karakterleri belirtiyoruz.
}).AddEntityFrameworkStores<DenemeContext>();


builder.Services.ConfigureApplicationCookie(x =>
{
    x.AccessDeniedPath = "/Home/Unauthorized";
    x.LoginPath = new PathString("/Login/Index");
    x.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    x.Cookie = new CookieBuilder
    {
        Name = "AspNetCoreIdentityExampleCookie", //Olu�turulacak Cookie'yi isimlendiriyoruz.
    };
    x.SlidingExpiration = true; //Expiration s�resinin yar�s� kadar s�re zarf�nda istekte bulunulursa e�er geri kalan yar�s�n� tekrar s�f�rlayarak ilk ayarlanan s�reyi tazeleyecektir.    
                                //x.AccessDeniedPath = new PathString(); //Yetkilendirme

});

builder.Services.AddLogging();
builder.Logging.AddSerilog();

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Log/log.txt",LogEventLevel.Warning)
    .CreateLogger();
           



// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


try
{
    app.Run();
}
catch (Exception e)
{

    Log.Error(e, "Startup error");
}

