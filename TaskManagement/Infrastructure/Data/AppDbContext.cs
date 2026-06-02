
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;


public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<TaskItem> TeskItems { set; get; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}