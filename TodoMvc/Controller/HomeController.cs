using Microsoft.AspNetCore.Mvc;
using TodoMvc.Data;
using TodoMvc.Models;

namespace TodoMvc.Controller;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Get([FromServices] TodoDbContext toDoContext)    
        => Ok(toDoContext.ToDos.ToList());

    [HttpGet("/{id:int}")]
    public IActionResult GetById([FromRoute]int id, [FromServices] TodoDbContext toDoContext)
    {
        var toDo = toDoContext.ToDos.FirstOrDefault(x => x.Id == id);                

        return Ok(toDo);
    }

    [HttpPost("/")]
    public IActionResult Post(ToDoModel toDo, [FromServices] TodoDbContext toDoContext)
    {
        toDoContext.ToDos.Add(toDo);
        
        toDoContext.SaveChanges();

        return Created($"/{toDo.Id}", toDo);
    }

    [HttpPut("/{id:int}")]
    public IActionResult Put([FromRoute]int id,
                             [FromBody]ToDoModel toDo, 
                             [FromServices] TodoDbContext toDoContext)
    {
        var model = toDoContext.ToDos.FirstOrDefault(x => x.Id == id); 

        if(model==null)
            return NotFound();

        model.Title = toDo.Title;               
        model.Done = toDo.Done;
        
        toDoContext.ToDos.Update(model);
        toDoContext.SaveChanges();

        return Ok(model);
    }
    [HttpDelete("/{id:int}")]
    public IActionResult Delete([FromRoute]int id,                              
                                [FromServices] TodoDbContext toDoContext)
    {
        var model = toDoContext.ToDos.FirstOrDefault(x => x.Id == id); 

        if(model==null)
            return NotFound();

        toDoContext.ToDos.Remove(model);                
        toDoContext.SaveChanges();

        return Ok("Registro apagado com sucesso");
    }
}