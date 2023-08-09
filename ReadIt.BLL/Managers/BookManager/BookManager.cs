using Mapster;
using Microsoft.EntityFrameworkCore;
using ReadIt.BLL.Models.BookModels;
using ReadIt.DAL.Entities;
using ReadIt.DAL.Persistance;

namespace ReadIt.BLL.Managers.BookManager
{
    public class BookManager : IBookManager
    {
        protected ReadItDbContext _dataContext;

        public BookManager(ReadItDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<BookViewModel> GetAll()
        {
            var books = _dataContext.Books.AsNoTracking().ProjectToType<BookViewModel>();
            return books;
        }
        public async Task<BookViewModel> GetByIdAsync(Guid id)
        {
            var bookById = await _dataContext.Books.AsNoTracking()
                      .Where(book => book.Id == id)
                      .ProjectToType<BookViewModel>()
                          .FirstOrDefaultAsync()
           ?? throw new Exception("The book with such id doesn't exist");
            return bookById;
        }

        public async Task<BookViewModel> GetByTitleAsync(string title)
        {
            var bookByTitle = await _dataContext.Books.AsNoTracking()
                      .Where(book => book.Title == title)
                      .ProjectToType<BookViewModel>()
                          .FirstOrDefaultAsync()
           ?? throw new Exception("The book with such title doesn't exist");
            return bookByTitle;
        }
        public async Task<BookViewModel> CreateAsync(BookCreateModel model)
        {
            var book = model.Adapt<Book>();
            _dataContext.Books.Add(book);
            await _dataContext.SaveChangesAsync();
            return book.Adapt<BookViewModel>();

            //_dataContext.Books.Add(model.Adapt<Book>());
            //     await _dataContext.SaveChangesAsync();
            //     return model;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var bookById = await _dataContext.Books
                          .Where(book => book.Id == id)
                          .FirstOrDefaultAsync()
           ?? throw new Exception("The book with such id doesn't exist");

            _dataContext.Entry(bookById).State = EntityState.Deleted;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsExists(Guid id)
        {
            return await _dataContext.Books.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> IsExists(string title)
        {
            return await _dataContext.Books.AnyAsync(p => p.Title == title);
        }

        public async Task<BookViewModel> UpdateAsync(BookUpdateModel model)
        {
            //var bookById = await _dataContext.Books
            //.Where(book => book.Id == bookId)
            //.FirstOrDefaultAsync()
            //?? throw new Exception("The book with such id doesn't exist");

            var book = model.Adapt<Book>();
            _dataContext.Entry(book).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return book.Adapt<BookViewModel>();

            //_dataContext.Books.Update(model.Adapt<Book>());
            //await _dataContext.SaveChangesAsync();
            //return model;
        }
    }

}
