using BookStore.Models.Models;

namespace BookStore.DL.InMemoryDb
{
    public static class StaticData
    {
        public static List<Book> Books = new List<Book>()
        {
            new Book()
            {
                Id = 1,
                Title = "Book 1"
            },
            new Book()
            {
                Id = 2,
                Title = "Book 2"
            },
            new Book()
            {
                Id = 3,
                Title = "Book 3"
            }
        };
    }
}