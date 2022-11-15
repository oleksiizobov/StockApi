using Microsoft.EntityFrameworkCore;
using StockData.Objects;
using StockTestAPI.Infrastructure.Repositories;
using StockTestAPI.Infrastructure.Repositories.Interfaces;
using StockTestAPI.Services;
using StockTestAPI.Services.Interfaces;
using System.Configuration;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


IConfiguration Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ICacheHelperService, CacheHelperService>();
builder.Services.AddTransient<IStockReportModel, StockReportModel>();
builder.Services.AddTransient<IStockApiClientService, StockApiClientProxyRepository>();
builder.Services.AddTransient<IStockHistoryRepository, StockHistoryRepository>();
builder.Services.AddTransient<IStockHistoryProxyRepository, StockHistoryRepositoryProxyService>();
builder.Services.AddTransient<IStockReportService, StockReportService>();

var connectionString = Configuration.GetConnectionString("StockDb");
builder.Services.AddDbContext<StockDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDbContextFactory<StockDbContext>(
//     options =>
//         options..UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test"));

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
