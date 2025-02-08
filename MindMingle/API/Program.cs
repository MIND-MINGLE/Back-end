using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Application;
using Application.Interface;
using Application.Services;
using Application.MyMapper;
using Application.Library;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<MMDbContext>(options =>
  options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("*")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
//Add twilio "secret keys" from Appsetting
builder.Services.Configure<TwilioOptions>(builder.Configuration.GetSection("Twilio"));
// Add Automapper
builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);
// Inject the Repository
builder.Services.AddTransient<IUnitOfWorks, UnitOfWorks>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITwilioService, TwilioService>();
//builder.Services.AddScoped<ISignalRService, SignalRService>();


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
app.MapHub<SignalRService>("/chathub");

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
