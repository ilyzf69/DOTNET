namespace BookStoreAPI.Models
{
    public class BookCreateRequestDto
    {
        public string Title { get; init; } = default!;
        public string? Author { get; set; }
        public string Abstract { get; set; } = string.Empty;
    }
}