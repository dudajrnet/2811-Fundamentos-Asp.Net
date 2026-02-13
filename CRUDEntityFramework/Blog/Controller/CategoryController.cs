using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controller
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync();

            return Ok(categories);
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            var category = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(category => category.Id == id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost("v1/categories/")]
        public async Task<IActionResult> PostAsync(
            [FromBody] Category category,
            [FromServices] BlogDataContext context)
        {
            await context.Categories.AddAsync(category);
            
            await context.SaveChangesAsync();
            
            return Created($"v1/categories/{category.Id}", category);
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromBody] Category model,
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            var category = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(category => category.Id == id);

            if (category == null)
                return NotFound();

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);            
            
            await context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(            
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            var category = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(category => category.Id == id);

            if (category == null)
                return NotFound();           

            context.Categories.Remove(category);

            await context.SaveChangesAsync();

            return Ok("Registro apagado com sucesso!");
        }
    }
}
