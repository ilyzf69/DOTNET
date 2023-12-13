namespace BookStoreAPI.Models;

public class BookDto
{
    public string Title { get; init; } = default!;
    public string? Author { get; set; }
}