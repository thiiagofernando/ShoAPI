using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1/categoria")]
    public class CategoriaController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Categoria>>> Get([FromServices] DataContext context)
        {
            var categorias = await context.Categoria.AsNoTracking().ToListAsync();
            return Ok(categorias);
        }

        [HttpGet]
        [Route("id:int")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Categoria>> GetById(int id, [FromServices] DataContext context)
        {
            var categoria = await context.Categoria.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(categoria);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Categoria>> Post([FromServices] DataContext context, [FromBody] Categoria model
            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categoria.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar a categoria" });
            }
        }


        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Categoria>> Put(int id, [FromBody] Categoria model, [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada" });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {

                context.Entry<Categoria>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar a categoria" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Categoria>> Delete(int id, [FromServices] DataContext context)
        {
            var categoria = await context.Categoria.FirstOrDefaultAsync(x => x.Id == 1);
            if (categoria == null)
                return NotFound(new { message = "Categoria não encontrada" });

            try
            {
                context.Categoria.Remove(categoria);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida encontrada" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover a categoria" });
            }

        }
    }
}