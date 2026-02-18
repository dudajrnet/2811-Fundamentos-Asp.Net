using Blog.Data;
using Blog.Models;
using Blog.ViewModel;
using Blog.ViewModel.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controller
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("/v1/posts")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
            ) 
        {
            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();

                var posts = await context.Posts
                      .AsNoTracking()
                      .Include(post => post.Category)
                      .Include(post => post.Author)
                      .Select(post => new ListPostViewModel
                      {
                          Id = post.Id,
                          Title = post.Title,
                          Slug = post.Slug,
                          LastUpdateDate = post.LastUpdateDate,
                          Category = post.Category.Name,
                          Author = post.Author.Name
                      })
                      .Skip(page * pageSize)
                      .Take(pageSize)
                      .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                        {
                          count,
                          page,
                          pageSize,
                          posts
                        }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("A011 - Falha interna do servidor."));                
            }
        }

        [HttpGet("/v1/post/{id:int}")]
        public async Task<IActionResult> DetailsAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id
            )
        {
            try
            {
                var post = await context.Posts
                    .AsNoTracking()
                    .Include(post => post.Author)
                    .ThenInclude(author => author.Roles)
                    .Include(post => post.Category)
                    .FirstOrDefaultAsync(post => post.Id == id);

                if (post == null)
                    return NotFound(new ResultViewModel<string>("Conteúdo não encontrado."));

                return Ok(new ResultViewModel<Post>(post));                    
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("A013 - Falha interna do servidor."));
            }
        }

        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetPostByCategory(
            [FromRoute] string category,
            [FromServices] BlogDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
            ) 
        {
            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();

                var posts = await context
                    .Posts
                    .AsNoTracking()
                    .Include(post => post.Author)
                    .Include(post => post.Category)
                    .Where(post => post.Category.Slug == category)
                    .Select(post => new ListPostViewModel
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Slug = post.Slug,
                        LastUpdateDate = post.LastUpdateDate,
                        Category = post.Category.Name,
                        Author = $"{post.Author.Name} ({post.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(post => post.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>(" A014 - Falha interna do servidor."));
            }
        }
    }
}
