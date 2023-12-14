namespace BookStoreAPI.Models;

public class AuthorDto
{
    public string Title { get; init; } = default!;
    public string? Book { get; set; }
}