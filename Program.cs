using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmallUrl.Data;

var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    var devConnString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(devConnString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
else if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    var dockerConnString = builder.Configuration.GetConnectionString("DockerConnection")
        ?? throw new InvalidOperationException("Connection string 'DockerConnection' not found.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(dockerConnString));
}
else
{
    var prodConnString = Environment.GetEnvironmentVariable("DB_CONN")
        ?? throw new InvalidOperationException("Environment variable 'DB_CONN' not found.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(prodConnString));
}

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
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

app.MapFallback(async (HttpContext context, ApplicationDbContext db) =>
{
    var slug = context.Request.Path.Value?.TrimStart('/');

    if (string.IsNullOrWhiteSpace(slug))
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Not found.");
        return;
    }

    var link = await db.ShortLink.FirstOrDefaultAsync(l => l.Slug == slug);

    if (link is null)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Link not found.");
    }
    else
    {
        context.Response.Redirect(link.Destination);
    }
});

app.Run();
