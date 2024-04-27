using Microsoft.EntityFrameworkCore;
using SmartMaintApi.Interceptors;
using SmartMaintApi.Models;

namespace SmartMaintApi.Data
{
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(new UpdateEntityInfoInterceptor());
}
}