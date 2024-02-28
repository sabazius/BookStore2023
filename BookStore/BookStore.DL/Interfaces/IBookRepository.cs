using BookStore.Models.Models;

namespace BookStore.DL.Interfaces
{
    public interface IBookRepository
    {
        public Task AddBook(Book book);

        public Task DeleteBook(int id);

        public Task UpdateBook(Book book);

        public Task<Book?> GetBook(int id);

        public Task<List<Book>> GetAllBooks();

        public Task<List<Book>> GetAllBooksByAuthorId(int id);
    }
}
