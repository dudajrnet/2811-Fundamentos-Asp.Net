using Blog.Data;
using Blog.Extentions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModel;
using Blog.ViewModel.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace Blog.Controller
{
    [ApiController]    
    public class AccountController : ControllerBase
    {
        [HttpPost("/v1/account/")]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] RegisterViewModel model,
            [FromServices] BlogDataContext context,
            [FromServices] EmailService emailService
            ) 
        { 
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<List<string>>(ModelState.GetErrors()));

            try
            {
                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Slug = model.Email.Replace("@", "-").Replace(".", "-")
                };

                var password = PasswordGenerator.Generate();
                user.PasswordHash = PasswordHasher.Hash(password);

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                emailService.Send("DudaJunior",
                                  "dudajr.net@gmail.com", 
                                  "Teste Email", 
                                  $"A sua senha é {password}");

                return Ok(new ResultViewModel<dynamic>(new 
                {                    
                    user = user.Email, password                
                }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("A007 - Este E-mail já está cadastrado"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("A008 - Erro ao criar o usuário."));                
            }
        }
        
        [HttpPost("/v1/account/login")]       
        public async Task<IActionResult> LoginAsync(
            [FromBody] LoginViewModel model,
            [FromServices] BlogDataContext context,
            [FromServices] TokenServices tokenService) 
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<List<string>>(ModelState.GetErrors()));

            try
            {
                var user = context.Users
                          .AsNoTracking()
                          .Include(user => user.Roles)
                          .FirstOrDefault(user => user.Email == model.Email);

                if (user == null)
                    return StatusCode(401, new ResultViewModel<string>("Usuário ou E-mail inválido."));

                if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                    return StatusCode(401, new ResultViewModel<string>("Usuário ou E-mail inválido."));

                var token = tokenService.GenerateToken(user);

                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("A009 - Falha interna no servidor."));
            }                       
        }

        [Authorize]
        [HttpPost("/v1/account/uplod-image")]
        public async Task<ActionResult> UpLoadImageAsync(
            [FromBody] UpLoadImageViewModel model,
            [FromServices] BlogDataContext context
            )
        {
            var fileName = $"{Guid.NewGuid().ToString()}.jpg";

            try
            {               
                var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
                var bytes = Convert.FromBase64String(data);

                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);

                var user = context.Users
                .AsNoTracking()
                .FirstOrDefault(user => user.Email == User.Identity.Name);

                if (user == null)
                    return NotFound(new ResultViewModel<string>("Usuário não encontrado."));

                user.Image = $"https://localhost:7024/images/{fileName}";

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Imagem adicionada com sucesso.", null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("A0010 - Falha interna do servidor"));
            }            
        }

        
        //[Authorize(Roles = "user")]
        //[HttpGet("v1/user")]
        //public IActionResult GetUser() => Ok(User.Identity.Name);
        
        //[Authorize(Roles = "author")]
        //[HttpGet("v1/author")]
        //public IActionResult GetAuthor() => Ok(User.Identity.Name);

        //[Authorize(Roles = "admin")]
        //[HttpGet("v1/admin")]
        //public IActionResult GetAdmin() => Ok(User.Identity.Name);
    }
}
