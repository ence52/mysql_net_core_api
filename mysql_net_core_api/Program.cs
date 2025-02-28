using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using mysql_net_core_api;
using mysql_net_core_api.Caching;
using mysql_net_core_api.Core.Interfaces;
using mysql_net_core_api.Repositories;
using mysql_net_core_api.Services;
using mysql_net_core_api.Services.Auth;
using mysql_net_core_api.Services.JWT;
using mysql_net_core_api.Services.User;
using Serilog;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//Logger Configuration
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog();

// Add services to the container.


//AutoMapper
builder.Services.AddAutoMapper(typeof(Mapper));

//Mysql Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"), new MySqlServerVersion(new Version(8, 0, 41))));

//RedisConfiguration
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));
builder.Services.AddScoped<ICacheService,CacheService>();

//User Configuration
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

//Generic Repo
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//Generic Service
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));


//JWT Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.UseSecurityTokenValidators = true;
    opt.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = false, ValidateAudience = false, ValidateLifetime = false,ValidateIssuerSigningKey=true ,IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT")["Key"]!))};
    

});
builder.Services.AddAuthorization();
builder.Services.AddScoped<IJWTService, JWTService>();
//Auth
builder.Services.AddScoped<IAuthService, AuthService>();



builder.Services.AddControllers();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
