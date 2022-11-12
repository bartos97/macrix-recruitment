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

        /// <summary>
        /// Get all entities from database
        /// </summary>
        /// <returns>List of entities</returns>
        /// <response code="200">The request succeeded and data has been transfered</response>
        /// <response code="404">There is a problem with database creation</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<PersonEntity>>> GetPeopleEntities()
        {
            if (_context.peopleEntities == null)
            {
                return NotFound();
            }
            return await _context.peopleEntities.ToListAsync();
        }

        /// <summary>
        /// Get entity with given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>`PersonEntity` with given ID from database</returns>
        /// <response code="200">The request succeeded and data has been transfered</response>
        /// <response code="404">Entity with given ID doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
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

        /// <summary>
        /// Updates entity with given id with provided data
        /// </summary>
        /// <param name="id">ID of entity to update</param>
        /// <param name="personEntity">Entity values to update</param>
        /// <returns></returns>
        /// <response code="204">The request succeeded</response>
        /// <response code="400">Given ID differs from the one in the model</response>
        /// <response code="404">Entity with given ID doesn't exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Inserts new entities (with id == 0) to database or updates existing ones (with id > 0)
        /// </summary>
        /// <param name="entities">Array of `PersonEntity` objects</param>
        /// <returns></returns>
        /// <response code="204">The request succeeded</response>
        [HttpPost("batchInsertUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PostBatchInsertUpdate(IEnumerable<PersonEntity> entities)
        {
            if (_context.peopleEntities == null)
            {
                return Problem("Entity set 'PeopleContext.peopleEntities'  is null.");
            }

            foreach (var item in entities)
            {
                _context.Entry(item).State = item.id > 0 ? EntityState.Modified : EntityState.Added;
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Adds new entity to database
        /// </summary>
        /// <param name="personEntity"></param>
        /// <returns>Newly inserted entity</returns>
        /// <response code="201">The request succeeded</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces("application/json")]
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

        /// <summary>
        /// Deletes specified entity from database
        /// </summary>
        /// <param name="id">ID of entity to delete</param>
        /// <returns></returns>
        /// <response code="204">The request succeeded</response>
        /// <response code="404">Entity with given ID doesn't exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
