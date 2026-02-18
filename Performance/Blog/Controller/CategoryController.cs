using Blog.Data;
using Blog.Extentions;
using Blog.Models;
using Blog.ViewModel;
using Blog.ViewModel.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controller
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context,
            [FromServices] IMemoryCache cache)
        {
            try
            {
                var categories = cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                });

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("A000 - Não foi possível listar a(s) categoria(s)!"));
            }
        }
        private List<Category> GetCategories(BlogDataContext context) 
        {
            return context.Categories.AsNoTracking().ToList();
        }

        [HttpGet("v1/categories/{id:int}")]//Todo
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
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok( new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("A001 - Não foi possível obter a categoria!"));
            }
        }

        [HttpPost("v1/categories/")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context)
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

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

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }                        
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("A003 - Não foi possível criar a categoria!"));
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
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);

                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("A004 - Não foi possível alterar a categoria!"));
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
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado."));

                context.Categories.Remove(category);

                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>("Registro apagado com sucesso!"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("A005 - Não foi possível apagar a categoria!"));
            }
        }
    }
}
