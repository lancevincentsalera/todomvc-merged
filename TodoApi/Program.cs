using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoApiContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


/* Minimal APIs */
/******************/

#region GET ALL TODO ITEMS
// GET ALL TODO ITEM IN THE TODO LIST
app.MapGet("api/todoitems", async (TodoApiContext db) =>
    await db.TodoItems.Select(todo => new TodoItemReadDto(todo.Id, todo.Title, todo.IsCompleted)).ToListAsync()
)
.WithName("GetAllTodoItems")
.WithTags("TodoList")
.Produces<List<TodoItemReadDto>>(StatusCodes.Status200OK)
.WithDescription("Retrieves all Todo items.");
#endregion



#region ADD TODO ITEM
// ADD A TODO ITEM
app.MapPost("/api/todoitems", async (TodoApiContext db, TodoItemCreateDto input) =>
{
    var todo = new TodoItem
    {
        Title = input.Title,
    };
    var newTodo = await db.TodoItems.AddAsync(todo);
    await db.SaveChangesAsync();
    return Results.Created($"api/todoitems/{newTodo.Entity.Id}", newTodo.Entity);
})
.WithName("AddTodoItem")
.WithTags("TodoList")
.Produces<TodoItemReadDto>(StatusCodes.Status201Created)
.WithDescription("Adds a new Todo item.");
#endregion



#region TOGGLE ALL ITEMS
// UPDATES ALL TODO ITEMS 
app.MapPut("/api/todoitems/toggle-all", async (TodoApiContext db, List<TodoItem> todoItems) =>
{
    if (todoItems.Count == 0) return Results.NotFound($"No items to update");

    foreach (var todo in todoItems)
    {
        var todoItem = await db.TodoItems.FindAsync(todo.Id);
        if (todoItem is null) return Results.NotFound($"Item with id: {todo.Id} does not exist");
        todoItem.IsCompleted = todo.IsCompleted;
    }

    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("UpdateTodoItems")
.WithTags("TodoList")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.WithDescription("Updates an existing Todo item by ID.");
#endregion




#region UPDATE TODO ITEM
// UPDATE THE SPECIFIED TODO ITEM GIVEN THE ID
app.MapPut("/api/todoitems/{id}", async (TodoApiContext db, int id, TodoItemUpdateDto todoUpdate) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return Results.NotFound($"Item with id: {id} does not exist");

    todo.Title = todoUpdate.Title;
    todo.IsCompleted = todoUpdate.IsCompleted;

    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("UpdateTodoItem")
.WithTags("TodoList")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.WithDescription("Updates an existing Todo item by ID.");
#endregion




#region DELETE TODO ITEM
// DELETE A TODO ITEM BY ID
app.MapDelete("/api/todoitems/{id}", async (TodoApiContext db, int id) =>
{
    var todoToBeDeleted = await db.TodoItems.FindAsync(id);
    if (todoToBeDeleted is null) return Results.NotFound($"Item with id: {id} does not exist.");

    db.TodoItems.Remove(todoToBeDeleted);

    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteTodoItem")
.WithTags("TodoList")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.WithDescription("Deletes a specified TodoItem by ID.");
#endregion




#region DELETE COMPLETED ITEMS
// CLEAR ALL COMPLETED ITEMS
app.MapDelete("/api/todoitems/completed", async (TodoApiContext db) =>
{
    var completedTodos = await db.TodoItems
       .Where(t => t.IsCompleted)
       .ToListAsync();
    if (completedTodos.Count == 0) return Results.NotFound("No completed items to delete.");

    db.TodoItems.RemoveRange(completedTodos);
    await db.SaveChangesAsync();

    var remainingTodos = await db.TodoItems
        .Select(t => new TodoItemReadDto(t.Id, t.Title, t.IsCompleted))
        .ToListAsync();

    return Results.Ok(remainingTodos);
})
.WithName("DeleteCompletedItems")
.WithTags("TodoList")
.Produces<IEnumerable<TodoItemReadDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithDescription("Deletes all completed Todo items.");
#endregion



app.Run();


record TodoItemReadDto(int Id, string Title, bool IsCompleted);
record TodoItemCreateDto(string Title);
record TodoItemUpdateDto(string Title, bool IsCompleted);

