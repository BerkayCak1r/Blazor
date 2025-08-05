using BlazorApp1.Services;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.EntityFrameworkCore;
//using BlazorApp1.Data; // ← Namespace projenle uyumlu olmalı

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
if (string.IsNullOrWhiteSpace(apiBaseUrl)) //Api adresi null kontrolü
    throw new InvalidOperationException("API Base URL appsettings.json içinde tanımlı değil.");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();


// EF Core bağlantısı
//builder.Services.AddDbContext<NorthwindContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
