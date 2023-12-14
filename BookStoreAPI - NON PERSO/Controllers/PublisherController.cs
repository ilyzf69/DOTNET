using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreAPI.Entities;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public PublisherController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PublisherDto>>> GetPublishers()
        {
            var publishers = await _dbContext.Publishers.ToListAsync();
            var publishersDto = _mapper.Map<List<PublisherDto>>(publishers);
            return Ok(publishersDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Publisher))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Publisher>> AddPublisher([FromBody] Publisher publisher)
        {
            if (publisher == null)
            {
                return BadRequest();
            }

            _dbContext.Publishers.Add(publisher);
            await _dbContext.SaveChangesAsync();

            return Created("api/publisher", publisher);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Publisher>> UpdatePublisher(int id, [FromBody] Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<Publisher>> DeletePublisher(int id)
        {
            var publisher = await _dbContext.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _dbContext.Publishers.Remove(publisher);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool PublisherExists(int id)
        {
            return _dbContext.Publishers.Any(e => e.Id == id);
        }
    }
}
