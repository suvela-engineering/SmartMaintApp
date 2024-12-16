using Microsoft.EntityFrameworkCore;
using SmartMaintApi.Data;
using SmartMaintApi.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200");

                          policy.WithMethods("GET", "POST", "PUT", "DELETE");

                          policy.WithHeaders("Content-Type");
                      });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

// Add ImageService and Configuration for Sync.com API
builder.Services.AddSingleton<ImageService>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
