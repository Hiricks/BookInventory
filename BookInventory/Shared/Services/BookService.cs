using BookInventory.Models;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.Shared.Services
{
    public class BookService
    {
        private readonly List<Book> books = new List<Book>();
        private int nextId = 1;
        private readonly IJSRuntime _jsRuntime;
        private bool _isInitialized = false;

        public BookService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                await LogAsync("BookService initialized");
                _isInitialized = true;
            }
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            await InitializeAsync();
            await LogAsync("GetBooks method called");
            return books;
        }

        public async Task<Book> GetBookAsync(int id)
        {
            await InitializeAsync();
            await LogAsync($"GetBook method called with id: {id}");
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                await LogAsync($"Book not found with id: {id}");
                throw new ArgumentException("Book not found with the specified ID.");
            }
            return book;
        }

        public async Task AddBookAsync(Book book)
        {
            await InitializeAsync();
            await LogAsync($"AddBook method called with book title: {book.Title}");
            book.Id = nextId++;
            books.Add(book);
            await LogAsync($"Book added successfully. New book count: {books.Count}");
        }

        public async Task UpdateBookAsync(Book book)
        {
            await InitializeAsync();
            await LogAsync($"UpdateBook method called with book id: {book.Id}");
            var existingBook = books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.Year = book.Year;
                existingBook.CoverImage = book.CoverImage;
                existingBook.Summary = book.Summary;
                await LogAsync("Book updated successfully");
            }
            else
            {
                await LogAsync($"Book not found for update with id: {book.Id}");
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            await InitializeAsync();
            await LogAsync($"DeleteBook method called with id: {id}");
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                await LogAsync("Book deleted successfully");
            }
            else
            {
                await LogAsync($"Book not found for deletion with id: {id}");
            }
        }

        private async Task LogAsync(string message)
        {
            if (_isInitialized)
            {
                await _jsRuntime.InvokeVoidAsync("console.log", $"BookService: {message}");
            }
        }
    }
}