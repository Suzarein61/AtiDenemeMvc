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
    x.Password.RequiredLength = 4; //En az kaç karakterli olmasý gerektiðini belirtiyoruz.
    x.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunluluðunu kaldýrýyoruz.
    x.Password.RequireLowercase = false; //Küçük harf zorunluluðunu kaldýrýyoruz.
    x.Password.RequireUppercase = false; //Büyük harf zorunluluðunu kaldýrýyoruz.
    x.Password.RequireDigit = false; //0-9 arasý sayýsal karakter zorunluluðunu kaldýrýyoruz.

    x.User.RequireUniqueEmail = true; //Email adreslerini tekilleþtiriyoruz.
    x.User.AllowedUserNameCharacters = "abcçdefghiýjklmnoöpqrsþtuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRSÞTUÜVWXYZ0123456789-._@+"; //Kullanýcý adýnda geçerli olan karakterleri belirtiyoruz.
}).AddEntityFrameworkStores<DenemeContext>();


builder.Services.ConfigureApplicationCookie(x =>
{
    x.AccessDeniedPath = "/Home/Unauthorized";
    x.LoginPath = new PathString("/Login/Index");
    x.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    x.Cookie = new CookieBuilder
    {
        Name = "AspNetCoreIdentityExampleCookie", //Oluþturulacak Cookie'yi isimlendiriyoruz.
    };
    x.SlidingExpiration = true; //Expiration süresinin yarýsý kadar süre zarfýnda istekte bulunulursa eðer geri kalan yarýsýný tekrar sýfýrlayarak ilk ayarlanan süreyi tazeleyecektir.    
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

