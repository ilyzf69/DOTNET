namespace BookStoreAPI.Entities
{
    public class Publisher
    {


        
        public int Id { get; set; }
        public required string Title { get; init; }
        public string? Book { get; set; }
        public string? Author { get; set; }
        public string? Name { get; set; }


        public Publisher()
        {
            Name = ""; 
        }


    }
}
