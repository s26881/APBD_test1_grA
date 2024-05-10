namespace BooksAPI.Models
{
    public class BookEditionModel
    {
        public int id { get; set; }
        public string bookTitle { get; set; }
        public string editionTitle { get; set; }
        public string publishingHouseName { get; set; }     
        public DateTime releaseDate { get; set; }
    }
}
