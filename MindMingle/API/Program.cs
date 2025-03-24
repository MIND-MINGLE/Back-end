using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Application;
using Application.Interface;
using Application.Services;
using Application.MyMapper;
using Application.Library;
using Microsoft.OpenApi.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using API.Middleware;
using Application.IRepository;
using Infrastructure.Repository;
using Application.Service;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.Get<AppSetting>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});
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
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "https://mindmingle202.vercel.app")
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

// Đăng ký Repository 
builder.Services.AddScoped<IPatientResponseRepository, PatientResponseRepository>();
builder.Services.AddScoped<IPatientSurveyRepository, PatientSurveyRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


// Đăng ký Services 
builder.Services.AddTransient<IUnitOfWorks, UnitOfWorks>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITwilioService, TwilioService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ITherapistService, TherapistService>();
builder.Services.AddScoped<IChatGroupService, ChatGroupService>();
builder.Services.AddScoped<IUsersInGroupService, UsersInGroupService>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICredentialService, CredentialService>();
builder.Services.AddScoped<IEmergencyEndService, EmergencyEndService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IPurchasedPackageService, PurchasedPackageService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();


builder.Services.AddScoped<IPatientSurveyService, PatientSurveyService>();
builder.Services.AddScoped<IPatientResponseService, PatientResponseService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton(configuration!);

//
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
			},
			new string[] { }
		}
	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.MapHub<SignalRService>("/chathub");

app.UseCors();

app.UseHttpsRedirection();
app.UseAuthentication();
// This middleware must be AFTER UseAuthentication and BEFORE UseAuthorization
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();


app.MapControllers();

app.Run();
