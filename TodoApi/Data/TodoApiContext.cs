using Microsoft.EntityFrameworkCore;
using TodoApi.Data.Models;

namespace TodoApi.Data;

public class TodoApiContext : DbContext
{
    public TodoApiContext(DbContextOptions<TodoApiContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}