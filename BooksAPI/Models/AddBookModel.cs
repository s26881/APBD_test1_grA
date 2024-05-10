using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class AddBookModel
    {
        [Required]
        public string bookTitle {  get; set; }

        [Required]
        public string editionTitle { get; set; }

        [Required]
        public int publishingHouseId { get; set; }

        [Required]
        public DateTime releaseDate { get; set; }
    }
}
