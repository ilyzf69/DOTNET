namespace BookStoreAPI.Entities
{
	public class Author
	{


		public int Id { get; set; }
		public string? Book { get; set; }
		public string Name { get; set; }


		public Author()
		{
			Name = ""; // Initialisation de Name dans le constructeur
		}


	}
}