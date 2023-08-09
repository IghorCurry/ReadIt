using ReadIt.BLL.Models.BookModels;

namespace ReadIt.BLL.Managers.BookManager
{
    internal interface IBookManager
    {
        public IQueryable<BookViewModel> GetAll();
        public Task<BookViewModel> GetByIdAsync(Guid id);
        public Task<BookViewModel> GetByTitleAsync(string title);
        public Task<BookViewModel> CreateAsync(BookCreateModel model);
        public Task<bool> IsExists(Guid id);
        public Task<bool> IsExists(string title);
        public Task<BookViewModel> UpdateAsync(BookUpdateModel model);
        public Task<bool> DeleteAsync(Guid id);
    }
}
