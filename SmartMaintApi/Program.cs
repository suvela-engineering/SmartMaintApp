using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartMaintApi.Data;
using SmartMaintApi.Models;
using SmartMaintApi.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          //   policy.WithOrigins("*"); works but not good
                          policy.WithOrigins("http://localhost:4200"); // TO DO: Put maybe to .env file etc

                          // Allow specific methods for CRUD operations (GET, POST, PUT, DELETE)
                          policy.WithMethods("GET", "POST", "PUT", "DELETE");

                          // Optionally, allow specific headers if needed (e.g., Content-Type)
                          policy.WithHeaders("Content-Type");
                      });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication
builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

// Add Authorization
builder.Services.AddAuthorizationBuilder();

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
// options => options.UseNpgsql(nvironment.GetEnvironmentVariable("DB_Connection")));

builder.Services.AddIdentityCore<User>()
   .AddEntityFrameworkStores<AppDbContext>()
   .AddApiEndpoints(); // Creates automatically endpoints for user account management

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

//app.MapIdentityApi<User>();
app.UseAuthorization();
//app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();
app.Run();

