using BookInventory.Models;


namespace BookInventory.Shared.Services
{
    public class BookService
    {
        private readonly List<Book> books = new List<Book>();

        private int nextId = 1;

        public IEnumerable<Book> GetBooks()
        {
            return books;
        }

        public Book GetBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id) ?? throw new ArgumentException("Book not found with the specified ID.");
            return book;
        }

        public void AddBook(Book book)
        {
            book.Id = nextId++;
            books.Add(book);
        }
        public void UpdateBook(Book book)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.Year = book.Year;
                existingBook.CoverImage = book.CoverImage;
                existingBook.Summary = book.Summary;
            }
        }
        public void DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
            }
        }
    }
}
