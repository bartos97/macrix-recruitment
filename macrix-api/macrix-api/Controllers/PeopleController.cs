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
        private readonly IPeopleDbContext _context;

        public PeopleController(IPeopleDbContext context)
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
            var result = await _context.GetAllAsync();
            if (result.Any())
                return result.ToList();
            else
                return NotFound();
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
            var personEntity = await _context.GetOneAsync(id);
            if (personEntity == null)
                return NotFound();
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
                return BadRequest();

            try
            {
                await _context.ModifyOneAsync(personEntity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EntityExists(id))
                    return NotFound();
                else
                    throw;
            }

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
            await _context.AddOneAsync(personEntity);
            return CreatedAtAction("PostPersonEntity", new { id = personEntity.id }, personEntity);
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
            var result = await _context.DeleteOneAsync(id);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Inserts new entities (with id == 0) to database or updates existing ones (with id > 0)
        /// </summary>
        /// <param name="entities">Array of `PersonEntity` objects</param>
        /// <returns></returns>
        /// <response code="204">The request succeeded</response>
        /// <response code="201">The request succeeded and newly created entities are transmitted</response>
        [HttpPost("batchInsertUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostBatchInsertUpdate(IEnumerable<PersonEntity> entities)
        {
            var newEntities = await _context.BatchInsertUpdateAsync(entities);
            if (newEntities.Any())
                return CreatedAtAction("PostBatchInsertUpdate", newEntities);
            else
                return NoContent();
        }
    }
}
