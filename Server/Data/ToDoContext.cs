using Microsoft.EntityFrameworkCore;
using Shared;

namespace Server.Data;

public sealed class ToDoContext : DbContext

{
	public ToDoContext(DbContextOptions<ToDoContext> options) : base(options){}

	public DbSet<ToDo> ToDos => Set<ToDo>();
}
