using TodoClient.Data.Dtos;
using TodoClient.Data.Models;

namespace TodoClient.Services;

public interface ITodoService
{
    Task<List<TodoItem>> LoadTodos();
    Task<TodoItem> AddTodoItem(TodoItemCreateDto todoItem);
    Task UpdateTodoItem(TodoItem todoItem);
    Task ToggleAllTodoItems(List<TodoItem> todoItems);
    Task DeleteTodoItem(int id);
    Task<List<TodoItem>> ClearCompleted();
}