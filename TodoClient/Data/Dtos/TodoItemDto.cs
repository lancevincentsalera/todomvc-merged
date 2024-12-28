namespace TodoClient.Data.Dtos;

public class TodoItemDto
{
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; } = false;
}