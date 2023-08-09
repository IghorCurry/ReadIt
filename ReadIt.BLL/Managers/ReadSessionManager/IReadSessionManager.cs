using ReadIt.BLL.Models.ReadSessionModels;

namespace ReadIt.BLL.Managers.ReadSessionManager
{
    internal interface IReadSessionManager
    {
        public Task<IQueryable<ReadSessionViewModel>> GetAllAsync();
        public Task<ReadSessionViewModel> GetByIdAsync(Guid id);
        public Task<IQueryable<ReadSessionViewModel>> GetAllByUserAsync(Guid userId);
        public Task<ReadSessionViewModel> CreateAsync(ReadSessionCreateModel model);
        public Task<bool> IsExists(Guid id);
        public Task<ReadSessionViewModel> UpdateAsync(ReadSessionUpdateModel model, Guid readSessionId);
        public Task<bool> DeleteAsync(Guid id);
    }
}
