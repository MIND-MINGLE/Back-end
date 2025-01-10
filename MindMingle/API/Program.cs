using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Application;
using Application.Interface;
using Application.Services;
using Application.MyMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<MMDbContext>(options =>
  options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))));

// Add Automapper
builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);
// Inject the Repository
builder.Services.AddTransient<IUnitOfWorks, UnitOfWorks>();

builder.Services.AddScoped<IAccountService, AccountService>();
//
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
