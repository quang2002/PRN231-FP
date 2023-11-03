using System.Text;
using FP_FAP.Middlewares;
using FP_FAP.Models;
using FP_FAP.Repositories;
using FP_FAP.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.secret.json", false, false);

builder.Services.AddSingleton<MongoDBContext>(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("DB");
    var database         = new MongoClient(connectionString).GetDatabase("prnfp");
    return new(database);
});

builder.Services.AddSingleton<IUserRepository, MongoUserRepository>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FP_FAP", Version = "v1" });

    var scheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name         = "Authorization",
        Description  = "JWT Authorization header using the Bearer scheme.",
        In           = ParameterLocation.Header,
        Type         = SecuritySchemeType.Http,
        Scheme       = JwtBearerDefaults.AuthenticationScheme,
        Reference = new()
        {
            Id   = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        },
    };

    c.AddSecurityDefinition(scheme.Reference.Id, scheme);

    c.AddSecurityRequirement(new()
    {
        [scheme] = Array.Empty<string>(),
    });
});

builder.Services
       .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

           options.TokenValidationParameters = new()
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