using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using macrix_api.Models;

namespace macrix_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleContext _context;

        public PeopleController(PeopleContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonEntity>>> GetpeopleEntities()
        {
          if (_context.peopleEntities == null)
          {
              return NotFound();
          }
            return await _context.peopleEntities.ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonEntity>> GetPersonEntity(long id)
        {
          if (_context.peopleEntities == null)
          {
              return NotFound();
          }
            var personEntity = await _context.peopleEntities.FindAsync(id);

            if (personEntity == null)
            {
                return NotFound();
            }

            return personEntity;
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonEntity(long id, PersonEntity personEntity)
        {
            if (id != personEntity.id)
            {
                return BadRequest();
            }

            _context.Entry(personEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/People
        [HttpPost]
        public async Task<ActionResult<PersonEntity>> PostPersonEntity(PersonEntity personEntity)
        {
          if (_context.peopleEntities == null)
          {
              return Problem("Entity set 'PeopleContext.peopleEntities'  is null.");
          }
            _context.peopleEntities.Add(personEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonEntity", new { id = personEntity.id }, personEntity);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonEntity(long id)
        {
            if (_context.peopleEntities == null)
            {
                return NotFound();
            }
            var personEntity = await _context.peopleEntities.FindAsync(id);
            if (personEntity == null)
            {
                return NotFound();
            }

            _context.peopleEntities.Remove(personEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonEntityExists(long id)
        {
            return (_context.peopleEntities?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
