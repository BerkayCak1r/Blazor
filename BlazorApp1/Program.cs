﻿using BlazorApp1.Services;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.EntityFrameworkCore;
//using BlazorApp1.Data; // ← Namespace projenle uyumlu olmalı

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7097/") });
builder.Services.AddScoped<ProductService>();


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
