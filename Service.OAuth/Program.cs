using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Mapping;
using FPT_Exchange_Utility.Settings;
using Microsoft.EntityFrameworkCore;
using Service.OAuth.Model;
using Wallet_API.Configurations;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
builder.Services.Configure<AppSetting>(configuration.GetSection("AppSetting"));
builder.Services.Configure<GoogleAuthSettings>(configuration.GetSection("Authentication:Google"));
builder.Services.AddControllers();
builder.Services.AddDbContext<FptExchangeDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddDependenceInjection();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(GeneralProfile));
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
