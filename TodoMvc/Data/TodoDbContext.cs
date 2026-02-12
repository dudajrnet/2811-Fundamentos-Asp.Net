using Microsoft.EntityFrameworkCore;
using TodoMvc.Models;

namespace TodoMvc.Data;

public class TodoDbContext : DbContext
{
    public DbSet<TodoModel> ToDos {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("DataSource=app.db;Cache=Shared");
    }
}