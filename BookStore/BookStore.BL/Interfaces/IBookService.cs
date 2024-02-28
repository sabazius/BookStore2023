using BookStore.Models.Models;

namespace BookStore.BL.Interfaces
{
    public interface IBookService
    {
         Task<List<Book>> GetAllBooksByAuthorId(int authorId);

         Task AddBook(Book book);

         Task<List<Book>> GetAll();

         Task DeleteBook(int id);

         Task<Book?> GetBookById(int id);

         Task UpdateBook(Book book);
    }
}
