using TodoMvc.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<TodoDbContext>();

var app = builder.Build();

app.MapControllers();

app.Run();
