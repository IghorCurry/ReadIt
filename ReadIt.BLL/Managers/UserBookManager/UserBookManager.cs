using Mapster;
using Microsoft.EntityFrameworkCore;
using ReadIt.BLL.Models.UserBookManager;
using ReadIt.DAL.Entities;
using ReadIt.DAL.Persistance;

namespace ReadIt.BLL.Managers.UserBookManager
{
    public class UserBookManager : IUserBookManager
    {
        protected ReadItDbContext _dataContext;

        public UserBookManager(ReadItDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<UserBookViewModel> CreateAsync(UserBookCreateModel model)
        {
            var userBook = model.Adapt<UserBook>();
            _dataContext.UserBooks.Add(userBook);
            await _dataContext.SaveChangesAsync();
            return userBook.Adapt<UserBookViewModel>();

            //_dataContext.UserBooks.Add(model.Adapt<UserBook>());
            //await _dataContext.SaveChangesAsync();
            //return model;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var userBookById = await _dataContext.UserBooks
                          .Where(userBook => userBook.Id == id)
                          .FirstOrDefaultAsync()
           ?? throw new Exception("The user-book with such id doesn't exist");

            _dataContext.Entry(userBookById).State = EntityState.Deleted;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsExists(Guid id)
        {
            return await _dataContext.UserBooks.AnyAsync(p => p.Id == id);
        }

        public async Task<UserBookViewModel> UpdateAsync(UserBookUpdateModel model, Guid userBookId)
        {
            var userBookById = await _dataContext.UserBooks
            .Where(userBook => userBook.Id == userBookId)
            .FirstOrDefaultAsync()
            ?? throw new Exception("The user-book with such id doesn't exist");

            var userBook = model.Adapt<UserBook>();
            _dataContext.Entry(userBook).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return userBook.Adapt<UserBookViewModel>();

            //_dataContext.UserBooks.Update(model.Adapt<UserBook>());
            //await _dataContext.SaveChangesAsync();
            //return model;
        }

        public async Task<UserBookViewModel> GetByIdAsync(Guid id)
        {
            var userBookById = await _dataContext.UserBooks.AsNoTracking()
                            .Where(userBook => userBook.Id == id)
                            .ProjectToType<UserBookViewModel>()
                            .FirstOrDefaultAsync()
             ?? throw new Exception("The user-book with such id doesn't exist");
            return userBookById;
        }

        public async Task<IQueryable<UserBookViewModel>> GetAllAsync()
        {
            var userBooks = _dataContext.UserBooks.AsNoTracking().ProjectToType<UserBookViewModel>();
            return userBooks;
        }
    }
}
