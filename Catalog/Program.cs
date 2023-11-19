using AutoMapper;
using Catalog.Database;
using Catalog.Database.Entities;
using Catalog.Mappings;
using Catalog.Models;
using Catalog.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Reflection;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .SetBasePath(Environment.CurrentDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog Api",
        Version = "v1"
    });
    var filePath = Path.Combine(AppContext.BaseDirectory, "CatalogApi.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("Sqlite")));

builder.Services.AddAutoMapper(typeof(CatalogProfile));
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CatalogProfile());
});

builder.Services.AddSingleton(mapperConfig.CreateMapper());
builder.Services.AddScoped<ICatalogService, CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
