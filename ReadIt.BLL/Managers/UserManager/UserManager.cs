using Mapster;
using Microsoft.EntityFrameworkCore;
using ReadIt.BLL.Models.UserModels;
using ReadIt.DAL.Entities;
using ReadIt.DAL.Persistance;

namespace ReadIt.BLL.Managers.UserManager
{
    public class UserManager : IUserManager
    {
        protected ReadItDbContext _dataContext;

        public UserManager(ReadItDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<UserViewModel> CreateAsync(UserCreateModel model)
        {
            var user = model.Adapt<User>();
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return user.Adapt<UserViewModel>();

            //_dataContext.Users.Add(model.Adapt<User>());
            //await _dataContext.SaveChangesAsync();
            //return model;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var userById = await _dataContext.Users
                          .Where(user => user.Id == id)
                          .FirstOrDefaultAsync()
           ?? throw new Exception("The user with such id doesn't exist");

            _dataContext.Entry(userById).State = EntityState.Deleted;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public IQueryable<UserViewModel> GetAllAsync()
        {
            var users = _dataContext.Users.AsNoTracking().ProjectToType<UserViewModel>();
            return users;
        }

        public async Task<UserViewModel> GetByIdAsync(Guid id)
        {
            var userById = await _dataContext.Users.AsNoTracking()
                            .Where(user => user.Id == id)
                            .ProjectToType<UserViewModel>()
                            .FirstOrDefaultAsync()
             ?? throw new Exception("The user with such id doesn't exist");
            return userById;
        }

        public async Task<bool> IsExists(Guid id)
        {
            return await _dataContext.Users.AnyAsync(p => p.Id == id);
        }

        public async Task<UserViewModel> UpdateAsync(UserUpdateModel model)
        {
            //var userById = await _dataContext.Users
            //.Where(user => user.Id == userId)
            //.FirstOrDefaultAsync()
            //?? throw new Exception("The user with such id doesn't exist");

            var user = model.Adapt<User>();
            _dataContext.Entry(user).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return user.Adapt<UserViewModel>();


            //_dataContext.Users.Update(model.Adapt<User>());
            //await _dataContext.SaveChangesAsync();
            //return model;
        }
    }
}
