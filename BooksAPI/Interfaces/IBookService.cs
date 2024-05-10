using BooksAPI.Models;

namespace BooksAPI.Interfaces
{
    public interface IBookService
    {
        Task<List<BookEditionModel>> GetEditions(int id);
        Task<int> CreateAsync(AddBookModel model);
    }
}
