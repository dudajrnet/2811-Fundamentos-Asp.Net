using Microsoft.EntityFrameworkCore;
using TodoMvc.Data.Mappings;
using TodoMvc.Models;

namespace TodoMvc.Data;

public class TodoDbContext : DbContext
{
    public DbSet<ToDoModel> ToDos {get; set;} = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("DataSource=app.db;Cache=Shared");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfiguration(new TodoModelMap());        
}