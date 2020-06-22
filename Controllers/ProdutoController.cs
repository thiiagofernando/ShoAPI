using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1/produto")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "employee")]
        #region Aplicar_Cache_Somente_Desse_Metodo
        [ResponseCache(VaryByHeader ="User-Agent",Location =  ResponseCacheLocation.Any, Duration = 30)]
        #endregion Aplicar_Cache_Somente_Desse_Metodo
        #region Remover_Cache_Somente_Desse_Metodo
        //[ResponseCache(Duration= 0,Location = ResponseCacheLocation.None, NoStore = true)]
        #endregion Remover_Cache_Somente_Desse_Metodo
        public async Task<ActionResult<List<Produto>>> Get([FromServices] DataContext context)
        {
            var Produtos = await context
                .Produto
                .Include(x => x.Categoria)
                .AsNoTracking()
                .ToListAsync();

            return Produtos;
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Produto>> Get(int id, [FromServices] DataContext context)
        {
            var Produtos = await context
                .Produto
                .Include(x => x.Categoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Produtos;
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Produto>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var Produtos = await context
                .Produto
                .Include(x => x.Categoria)
                .AsNoTracking()
                .Where(x => x.CategoriaId == id)
                .ToListAsync();

            return Produtos;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Produto>> Post([FromServices] DataContext context, [FromBody] Produto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Produto.Add(model);
                    await context.SaveChangesAsync();
                    return model;
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel salvar o Produto" });
            }
        }
    }
}