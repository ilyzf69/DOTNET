[ApiController]
[Route("api/[controller]")]
public class PublisherController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public PublisherController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Publisher>>> GetPublishers()
    {
        var Publishers = await _dbContext.Publishers.ToListAsync();
        return Ok(Publishers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Publisher>> GetPublisher(int id)
    {
        var Publisher = await _dbContext.Publishers.FindAsync(id);

        if (Publisher == null)
        {
            return NotFound();
        }

        return Ok(Publisher);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Publisher))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Publisher>> PostPublisher([FromBody] Publisher Publisher)
    {
        if (Publisher == null)
        {
            return BadRequest();
        }

        Publisher addedPublisher = await _dbContext.Publishers.FirstOrDefaultAsync(g => g.Name == Publisher.Name);
        if (addedPublisher != null)
        {
            return BadRequest("Publisher already exists");
        }
        else
        {
            await _dbContext.Publishers.AddAsync(Publisher);
            await _dbContext.SaveChangesAsync();

            return Created("api/Publisher", Publisher);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Publisher>> PutPublisher(int id, [FromBody] Publisher Publisher)
    {
        if (id != Publisher.Id)
        {
            return BadRequest("ID mismatch");
        }

        _dbContext.Entry(Publisher).State = EntityState.Modified;

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
        var Publisher = await _dbContext.Publishers.FindAsync(id);
        if (Publisher == null)
        {
            return NotFound();
        }

        _dbContext.Publishers.Remove(Publisher);
        await _dbContext.SaveChangesAsync();

        return Ok(Publisher);
    }

    private bool PublisherExists(int id)
    {
        return _dbContext.Publishers.Any(e => e.Id == id);
    }
}
