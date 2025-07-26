using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Data;

var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    var devConnString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(devConnString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
else
{
    var prodConnString = builder.Configuration.GetConnectionString("MsSqlConnection")
        ?? throw new InvalidOperationException("Connection string 'MsSqlConnection' not found.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(prodConnString));
}

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();


var app = builder.Build();

// Migrate the database on startup
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.EnsureCreated();
    // dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
