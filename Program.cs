using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Play.Data;
using Play.Models;
using Serilog;
#nullable disable

var builder = WebApplication.CreateBuilder(args);
var Constring = "Host=localhost;Port=5432;Database = RentToPlay2;Username = postgres;password = vibes2223";

var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
var validIssuer = jwtSettings["ValidIssuer"];
var validAudience = jwtSettings["ValidAudience"];

Log.Logger = new LoggerConfiguration()
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
       {

           loggingBuilder.AddConsole();
           loggingBuilder.AddConfiguration();

       });


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RentDbContext>(Options => Options.UseNpgsql(Constring));
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = validIssuer,
        ValidAudience = validAudience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("OwnerOrEmployeeorUser", policy =>
          policy.RequireRole("Owner", "Employee", "User"));
        options.AddPolicy("OwnerOrEmployee", policy =>
            policy.RequireRole("Owner", "Employee"));
        options.AddPolicy("Owner", policy =>
            policy.RequireRole("Owner"));
        options.AddPolicy("User", policy =>
            policy.RequireRole("User"));
              options.AddPolicy("Employee", policy =>
            policy.RequireRole("Employee"));
    });

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
