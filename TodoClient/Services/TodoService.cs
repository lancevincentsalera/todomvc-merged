using TodoClient.Data.Dtos;
using TodoClient.Data.Models;

namespace TodoClient.Services;

public class TodoService : ITodoService
{
    private readonly HttpClient _http;

    public TodoService(HttpClient http) => _http = http;

    public async Task<List<TodoItem>> LoadTodos()
    {
        var response = await _http.GetFromJsonAsync<List<TodoItem>>("todos");
        return response ?? new List<TodoItem>();
    }
    public async Task<TodoItem> AddTodoItem(TodoItemDto todoItem)
    {
        var response = await _http.PostAsJsonAsync("todos", todoItem);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoItem>()
                ?? throw new InvalidOperationException("Response deserialization failed.");
    }
    public async Task<TodoItem> UpdateTodoItem(TodoItem todoItem)
    {
        var response = await _http.PutAsJsonAsync($"todos/{todoItem.Id}", todoItem);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoItem>()
                ?? throw new InvalidOperationException("Response deserialization failed.");
    }
    public async Task ToggleAllTodoItems(List<TodoItem> todoItems)
    {
        var response = await _http.PutAsJsonAsync("todos/toggle-all", todoItems);
        response.EnsureSuccessStatusCode();
    }
    public async Task DeleteTodoItem(int id) => await _http.DeleteAsync($"todos/{id}");
    public async Task<List<TodoItem>> ClearCompleted()
    {
        var response = await _http.DeleteAsync("todos/clear-completed");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<TodoItem>>()
                ?? throw new InvalidOperationException("Response deserialization failed.");
    }
}