using Microsoft.EntityFrameworkCore;
using TodoApi.Data.Models;

namespace TodoApi.Data;

public class TodoApiContext(DbContextOptions<TodoApiContext> options) : DbContext(options)
{
    public required DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
        });
    }
}