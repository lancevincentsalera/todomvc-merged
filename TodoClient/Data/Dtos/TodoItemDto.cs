namespace TodoClient.Data.Dtos;

public record TodoItemCreateDto(string Title);
public record TodoItemUpdateDto(string Title, bool IsCompleted);