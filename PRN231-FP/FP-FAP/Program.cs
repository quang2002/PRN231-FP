using System.Text;
using FP_FAP.Middlewares;
using FP_FAP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.secret.json", false, false);

builder.Services.AddSingleton<IMongoDatabase>(_ =>
{
    var connectionString = builder.Configuration["MongoDB:ConnectionString"];
    var databaseName     = builder.Configuration["MongoDB:DatabaseName"];
    return new MongoClient(connectionString).GetDatabase(databaseName);
});

builder.Services.AddSingleton<UserCollection>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
       .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

           options.TokenValidationParameters = new TokenValidationParameters
           {
               IssuerSigningKey         = new SymmetricSecurityKey(key),
               ValidateIssuer           = false,
               ValidateAudience         = false,
               ValidateIssuerSigningKey = true,
               ValidateLifetime         = true,
           };
       });

builder.Services.AddAuthorization();

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
app.UseMiddleware<UserInfoMiddleware>();

app.MapControllers();

app.Run();