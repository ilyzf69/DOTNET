[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AuthorController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAuthors()
    {
        var Authors = await _dbContext.Authors.ToListAsync();
        return Ok(Authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        var Author = await _dbContext.Authors.FindAsync(id);

        if (Author == null)
        {
            return NotFound();
        }

        return Ok(Author);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Author))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Author>> PostAuthor([FromBody] Author Author)
    {
        if (Author == null)
        {
            return BadRequest();
        }

        Author addedAuthor = await _dbContext.Authors.FirstOrDefaultAsync(g => g.Name == Author.Name);
        if (addedAuthor != null)
        {
            return BadRequest("Author already exists");
        }
        else
        {
            await _dbContext.Authors.AddAsync(Author);
            await _dbContext.SaveChangesAsync();

            return Created("api/Author", Author);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Author>> PutAuthor(int id, [FromBody] Author Author)
    {
        if (id != Author.Id)
        {
            return BadRequest("ID mismatch");
        }

        _dbContext.Entry(Author).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(id))
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
    public async Task<ActionResult<Author>> DeleteAuthor(int id)
    {
        var Author = await _dbContext.Authors.FindAsync(id);
        if (Author == null)
        {
            return NotFound();
        }

        _dbContext.Authors.Remove(Author);
        await _dbContext.SaveChangesAsync();

        return Ok(Author);
    }

    private bool AuthorExists(int id)
    {
        return _dbContext.Authors.Any(e => e.Id == id);
    }
}
