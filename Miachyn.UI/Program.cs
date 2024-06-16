using Miachyn.UI.Data;
using Miachyn.UI.Services.CategoryService;
using Miachyn.UI.Services.FurnitureService;
using Miachyn.UI.TagHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Serilog;
using Miachyn.UI.Middleware;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})    
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p =>
    p.RequireClaim(ClaimTypes.Role, "admin"));
});

builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
//builder.Services.AddScoped<IFurnitureService, MemoryFurnitureService>();

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt=>opt.BaseAddress = new Uri("https://localhost:7002/api/categories/"));
builder.Services.AddHttpClient<IFurnitureService, ApiFurnitureService>(opt => opt.BaseAddress = new Uri("https://localhost:7002/api/furnitures/"));

builder.Services.AddHttpContextAccessor();

// Добавляем сервисы для работы с сессиями
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Middleware для проверки зависимостей
app.Use(async (context, next) =>
{
    var httpContextAccessor = app.Services.GetService<IHttpContextAccessor>();
    if (httpContextAccessor == null)
    {
        throw new Exception("IHttpContextAccessor is not registered correctly.");
    }

    var linkGenerator = app.Services.GetService<LinkGenerator>();
    if (linkGenerator == null)
    {
        throw new Exception("LinkGenerator is not registered correctly.");
    }

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Включение использования сессий
app.UseSession();

app.UseFileLogger();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


await DbInit.SeedData(app);

app.Run();
