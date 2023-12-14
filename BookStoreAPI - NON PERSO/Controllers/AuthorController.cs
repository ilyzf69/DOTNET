using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreAPI.Entities; // Assure-toi que cette référence est correcte
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthorController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDto>>> GetAuthors()
        {
            var authors = await _dbContext.Authors.ToListAsync();

            var authorsDto = _mapper.Map<List<AuthorDto>>(authors);

            return Ok(authorsDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Author))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Author>> PostAuthor([FromBody] Author author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var addedAuthor = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == author.Name);
            if (addedAuthor != null)
            {
                return BadRequest("Author already exists");
            }

            _dbContext.Authors.Add(author);
            await _dbContext.SaveChangesAsync();

            return Created("author", author);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Author>> PutAuthor(int id, [FromBody] Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            var authorToUpdate = await _dbContext.Authors.FindAsync(id);

            if (authorToUpdate == null)
            {
                return NotFound();
            }

            authorToUpdate.Name = author.Name;
            // Continuez pour d'autres propriétés

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> DeleteAuthor(int id)
        {
            var authorToDelete = await _dbContext.Authors.FindAsync(id);

            if (authorToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Authors.Remove(authorToDelete);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
