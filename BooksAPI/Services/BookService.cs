using BooksAPI.Models;
using BooksAPI.Interfaces;
using Microsoft.Data.SqlClient;

namespace BooksAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IConfiguration _configuration;

        public BookService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<BookEditionModel>> GetEditions(int id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            string query = @"
                SELECT
                    be.PK AS Id,
                    b.title AS BookTitle,
                    be.edition_title AS EditionTitle,
                    ph.name AS PublishingHouseName,
                    be.release_date AS ReleaseDate
                FROM
                    books_editions be
                INNER JOIN
                    books b ON b.PK = be.FK_book
                INNER JOIN
                    publishing_houses ph ON ph.PK = be.FK_publishing_house;
            ";

            List<BookEditionModel> bookEditionList = new List<BookEditionModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookEditionModel bookEdition = new BookEditionModel
                            {
                                id = Convert.ToInt32(reader["Id"]),
                                bookTitle = reader["BookTitle"].ToString(),
                                editionTitle = reader["EditionTitle"].ToString(),
                                publishingHouseName = reader["PublishingHouseName"].ToString(),
                                releaseDate = Convert.ToDateTime(reader["ReleaseDate"])
                            };

                            bookEditionList.Add(bookEdition);
                        }
                    }
                }
            }

            return bookEditionList;
        }

        public async Task<int> CreateAsync(AddBookModel model)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            var addBookQuery = "INSERT INTO books (title) OUTPUT INSERTED.PK VALUES (@Title)";
            var addBookCommand = new SqlCommand(addBookQuery, connection);
            addBookCommand.Parameters.AddWithValue("@Title", model.bookTitle);

            int addedBookId = (int) addBookCommand.ExecuteScalar();

            var addBookEditionQuery = "INSERT INTO books_editions (FK_publishing_house, FK_book, edition_title, release_date) " +
                                                "VALUES (@PublishingHouse, @BookId, @EditionTitle, @ReleaseDate)";

            var addBookEditionCommand = new SqlCommand(addBookEditionQuery, connection);
            addBookCommand.Parameters.AddWithValue("@PublishingHouse", model.publishingHouseId);
            addBookCommand.Parameters.AddWithValue("@BookId", addedBookId);
            addBookCommand.Parameters.AddWithValue("@EditionTitle", model.editionTitle);
            addBookCommand.Parameters.AddWithValue("@ReleaseDate", model.releaseDate);

            int addedBookEditionId = (int) addBookEditionCommand.ExecuteScalar();

            return addedBookId;
        }
    }
}
