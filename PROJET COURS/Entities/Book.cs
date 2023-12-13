namespace BookStoreAPI.Entities
{
    public class Book
    {


        // Une prop mets a dispostion des accesseurs (getters et setters)
        // ceci est une property
        public int Id { get; set; }
        public required string Title { get; init; }
        public string? Author { get; set; }

        public string Client { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }



    }
}