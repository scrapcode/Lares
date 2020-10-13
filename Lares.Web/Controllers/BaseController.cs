using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Lares.Interfaces;
using Lares.Infrastructure;
using Lares.Entities;
using Lares.Infrastructure.Repositories;

namespace Lares.Controllers
{
    public abstract class BaseController<TEntity> : Controller
        where TEntity: class, IEntity
    {
        private readonly IRepository<TEntity> _repo;

        public BaseController(DataContext context)
        {
            _repo = new CoreRepository<TEntity>(context);
        }

        // GET: /[controller]/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            var result = await _repo.GetAll();
            return View(result);
        }

        // GET: /[controller]/[id]
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> Get(int id)
        {
            var result = await _repo.GetByIdAsync(id);

            if(result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: /[controller]/[id]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TEntity entity)
        {
            if(id != entity.Id)
            {
                return BadRequest();
            }

            await _repo.UpdateAsync(entity);
            return NoContent();
        }

        // POST: [controller]
        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
            await _repo.AddAsync(entity);
            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        // DELETE: [controller]/[id]
        [HttpDelete("{id}")]
        public async Task<ActionResult<TEntity>> Delete(int id)
        {
            var result = await _repo.DeleteAsync(id);
            
            if(result == null)
            {
                return NotFound();
            }

            return result;
        }
    }
}
