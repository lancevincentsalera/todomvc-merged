using System.ComponentModel.DataAnnotations;

namespace TodoApi.Data.Models;

public class TodoItem
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(150)]
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; } = false;
}