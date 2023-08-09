using ReadIt.BLL.Models.UserBookManager;

namespace ReadIt.BLL.Managers.UserBookManager
{
    internal interface IUserBookManager
    {
        public Task<IQueryable<UserBookViewModel>> GetAllAsync();
        public Task<UserBookViewModel> GetByIdAsync(Guid id);
        public Task<UserBookViewModel> CreateAsync(UserBookCreateModel model);
        public Task<bool> IsExists(Guid id);
        public Task<UserBookViewModel> UpdateAsync(UserBookUpdateModel model, Guid userBookId);
        public Task<bool> DeleteAsync(Guid id);
    }
}
