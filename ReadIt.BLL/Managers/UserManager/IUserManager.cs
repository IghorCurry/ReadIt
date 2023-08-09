using ReadIt.BLL.Models.UserModels;

namespace ReadIt.BLL.Managers.UserManager
{
    internal interface IUserManager
    {
        IQueryable<UserViewModel> GetAllAsync();
        Task<UserViewModel> GetByIdAsync(Guid id);
        Task<UserViewModel> CreateAsync(UserCreateModel model);
        Task<bool> IsExists(Guid id);
        Task<UserViewModel> UpdateAsync(UserUpdateModel model);
        Task<bool> DeleteAsync(Guid id);
    }
}
