using AutoMapper;
using Delivery.Database;
using Delivery.Mappings;
using Delivery.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

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
        Title = "Delivery Api",
        Version = "v1"
    });
    var filePath = Path.Combine(AppContext.BaseDirectory, "DeliveryApi.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpClient("CatalogService", c =>
{
    c.BaseAddress = new Uri("http://localhost:5160");
});

builder.Services.AddHttpClient("OrderService", c =>
{
    c.BaseAddress = new Uri("http://localhost:5165");
});

builder.Services.AddDbContext<DeliveryDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("Sqlite")));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new DeliveryProfile());
});

builder.Services.AddSingleton(mapperConfig.CreateMapper());
builder.Services.AddScoped<IDeliveryService, DeliveryService>();

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
