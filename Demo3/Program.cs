using System.Text;
using Demo.DAL;
using Demo3.AppCode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,          //是否驗證Issuer
            ValidateAudience = true,        //是否驗證Audience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateActor = true,
            ValidIssuer = builder.Configuration["AppSettings:JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["AppSettings:JwtSettings:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JwtSettings:SignKey"]!))
        };
    });

builder.Services.AddDbContext<TestDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TestDbConn")));

// 註冊 AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// 註冊 TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

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
