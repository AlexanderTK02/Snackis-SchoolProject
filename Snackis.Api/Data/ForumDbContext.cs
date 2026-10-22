using Microsoft.EntityFrameworkCore;
using Snackis.Api.Models;

namespace Snackis.Api.Data;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<ForumUser> Users { get; set; }
}