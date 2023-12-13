[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public GenreController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Genre>>> GetGenres()
    {
        var genres = await _dbContext.Genres.ToListAsync();
        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(int id)
    {
        var genre = await _dbContext.Genres.FindAsync(id);

        if (genre == null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Genre))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Genre>> PostGenre([FromBody] Genre genre)
    {
        if (genre == null)
        {
            return BadRequest();
        }

        Genre addedGenre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == genre.Name);
        if (addedGenre != null)
        {
            return BadRequest("Genre already exists");
        }
        else
        {
            await _dbContext.Genres.AddAsync(genre);
            await _dbContext.SaveChangesAsync();

            return Created("api/genre", genre);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Genre>> PutGenre(int id, [FromBody] Genre genre)
    {
        if (id != genre.Id)
        {
            return BadRequest("ID mismatch");
        }

        _dbContext.Entry(genre).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GenreExists(id))
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
    public async Task<ActionResult<Genre>> DeleteGenre(int id)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }

        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();

        return Ok(genre);
    }

    private bool GenreExists(int id)
    {
        return _dbContext.Genres.Any(e => e.Id == id);
    }
}
