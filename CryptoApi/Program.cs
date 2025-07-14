using CryptoApi.Data;
using CryptoApi.Models;
using CryptoApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add configuration
builder.Services.Configure<ScrapingConfig>(builder.Configuration.GetSection("ScrapingConfig"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=cryptodata.db"));


// Services (all scoped)
builder.Services.AddScoped<SqliteDataStorage>();
builder.Services.AddScoped<ScrapingService>();

// HTTP Client
builder.Services.AddHttpClient();

builder.Services.AddHostedService<ScrapingBackgroundService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificAndLocalhost", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            // Allow any localhost and one specific domain
            return origin.StartsWith("http://localhost") ||
                   origin.StartsWith("https://localhost") ||
                   origin == "https://thedex.codeit.com.ng";
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();  // Applies pending migrations
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificAndLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
