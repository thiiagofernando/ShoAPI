using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Shop.Models;
using Shop.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Shop.Controllers
{
    [Route("v1/usuario")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<Usuario>>> GetAction([FromServices] DataContext context)
        {
            var user = await context
                        .Usuario
                        .AsNoTracking()
                        .ToListAsync();
            return user;
        }


        [HttpPost]
        [Route("")]
        [Authorize(Roles="manager")]
        //[AllowAnonymous]
        public async Task<ActionResult<Usuario>> Post([FromServices] DataContext context, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                usuario.Role = "employee";
                usuario.Password = PasswordService.GeneratePassword(usuario.Password);
                context.Usuario.Add(usuario);
                await context.SaveChangesAsync();
                return usuario;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Usuario>> Put([FromServices] DataContext context, int id, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != usuario.Id)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }
            try
            {

                context.Entry(usuario).State = EntityState.Modified;
                if (context.Entry(usuario).State == EntityState.Modified)
                {
                    usuario.Password = PasswordService.GeneratePassword(usuario.Password);
                }
                await context.SaveChangesAsync();
                return usuario;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar o usuário" });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] Usuario usuario)
        {
            try
            {
                string hash = PasswordService.GeneratePassword(usuario.Password);
                var user = await context.Usuario
                                .AsNoTracking()
                                .Where(x => x.Username == usuario.Username && 
                                x.Password == hash)
                                .FirstOrDefaultAsync();
                if (user == null)
                    return NotFound(new { message = "Usuário ou senha inválidos" });

                var token = TokenService.GenerateToken(user);
                return new
                {
                    user = user,
                    token = token
                };
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível realizar o login" });
            }
        }
    }
}