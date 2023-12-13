[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ClientController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Client>>> GetClients()
    {
        var Clients = await _dbContext.Clients.ToListAsync();
        return Ok(Clients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var Client = await _dbContext.Clients.FindAsync(id);

        if (Client == null)
        {
            return NotFound();
        }

        return Ok(Client);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Client))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Client>> PostClient([FromBody] Client Client)
    {
        if (Client == null)
        {
            return BadRequest();
        }

        Client addedClient = await _dbContext.Clients.FirstOrDefaultAsync(g => g.Name == Client.Name);
        if (addedClient != null)
        {
            return BadRequest("Client already exists");
        }
        else
        {
            await _dbContext.Clients.AddAsync(Client);
            await _dbContext.SaveChangesAsync();

            return Created("api/Client", Client);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Client>> PutClient(int id, [FromBody] Client Client)
    {
        if (id != Client.Id)
        {
            return BadRequest("ID mismatch");
        }

        _dbContext.Entry(Client).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(id))
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
    public async Task<ActionResult<Client>> DeleteClient(int id)
    {
        var Client = await _dbContext.Clients.FindAsync(id);
        if (Client == null)
        {
            return NotFound();
        }

        _dbContext.Clients.Remove(Client);
        await _dbContext.SaveChangesAsync();

        return Ok(Client);
    }

    private bool ClientExists(int id)
    {
        return _dbContext.Clients.Any(e => e.Id == id);
    }
}
