using Microsoft.EntityFrameworkCore;
using TikTokAPI.Models;

namespace TikTokAPI.Database;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Article> Articles { get; set; }
}