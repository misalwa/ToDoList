using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options=>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ToDoContext>(options => options.UseSqlite("Data Source=./Data/ToDo.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoApp WebAPI");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseCors();



app.MapGet("todos/all", async (ToDoContext dbContext) =>
{
    List<ToDo> allToDos = await dbContext.ToDos.ToListAsync();
    return allToDos;
});

app.MapGet("todos/by-id/{Id}", async (int Id, ToDoContext dbContext) =>
{
    ToDo? todo = await dbContext.ToDos.FindAsync(Id);

    if (todo != null)
    {
        return Results.Ok(todo);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("todos/create/", async (ToDo toDoToCreate, ToDoContext dbContext) =>
{
    dbContext.ToDos.Add(toDoToCreate);
    await dbContext.SaveChangesAsync();
    return Results.Created($"todos/by-id/{toDoToCreate.Id}", toDoToCreate);
});

app.MapPut("todos/update/{Id}", async (int Id, ToDo updatedToDo, ToDoContext dbContext) =>
{
    var toDoToUpdate = await dbContext.ToDos.FindAsync(Id);

    if (toDoToUpdate == null)
    {
        return Results.NotFound();
    }

    toDoToUpdate.Content = updatedToDo.Content;
    toDoToUpdate.IsDone = updatedToDo.IsDone;

    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("todos/delete/{Id}", async (int Id, ToDoContext dbContext) =>
{
    ToDo? toDoToDelete = await dbContext.ToDos.FindAsync(Id);

    if (toDoToDelete != null)
    {
        dbContext.ToDos.Remove(toDoToDelete);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
    else
    {
        return Results.NotFound();
    }
});



app.Run();
