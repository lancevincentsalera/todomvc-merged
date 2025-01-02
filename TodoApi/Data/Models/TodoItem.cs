using System.ComponentModel.DataAnnotations;

namespace TodoApi.Data.Models;

public record TodoItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
}