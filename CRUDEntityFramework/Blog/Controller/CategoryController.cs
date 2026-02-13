using Blog.Data;
using Blog.Models;
using Blog.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context.Categories
                            .AsNoTracking()
                            .FirstOrDefaultAsync(category => category.Id == id);

                if (category == null)
                    return NotFound();

                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(500, "A001 - Não foi possível obter a categoria!");
            }
        }

        [HttpPost("v1/categories/")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context)
        {
            try
            {
                var category = new Category()
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug,                    
                };
                                        
                await context.Categories.AddAsync(category);

                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", category);
            }
            catch (SqlException)
            {
                return StatusCode(500, "A002 - Não foi possível criar a categoria!");
            }
            catch (Exception)
            {
                return StatusCode(500, "A003 - Não foi possível criar a categoria!");
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            try
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
            catch (Exception)
            {
                return StatusCode(500, "A004 - Não foi possível alterar a categoria!");
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(            
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            try
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
            catch (Exception)
            {
                return StatusCode(500, "A001 - Não foi possível apagar a categoria!");
            }
        }
    }
}
