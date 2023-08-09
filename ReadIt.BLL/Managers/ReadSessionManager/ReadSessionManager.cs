using Mapster;
using Microsoft.EntityFrameworkCore;
using ReadIt.BLL.Models.ReadSessionModels;
using ReadIt.DAL.Entities;
using ReadIt.DAL.Persistance;

namespace ReadIt.BLL.Managers.ReadSessionManager
{
    public class ReadSessionManager : IReadSessionManager
    {
        protected ReadItDbContext _dataContext;

        public ReadSessionManager(ReadItDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ReadSessionViewModel> CreateAsync(ReadSessionCreateModel model)
        {
            var readSession = model.Adapt<ReadSession>();
            _dataContext.ReadSessions.Add(readSession);
            await _dataContext.SaveChangesAsync();
            return readSession.Adapt<ReadSessionViewModel>();

            //_dataContext.ReadSessions.Add(model.Adapt<ReadSession>());
            //await _dataContext.SaveChangesAsync();
            //return model;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var readSessionById = await _dataContext.ReadSessions
                          .Where(readSession => readSession.Id == id)
                          .FirstOrDefaultAsync()
           ?? throw new Exception("The reading session with such id doesn't exist");

            _dataContext.Entry(readSessionById).State = EntityState.Deleted;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<ReadSessionViewModel> GetByIdAsync(Guid id)
        {
            var readSessionById = await _dataContext.ReadSessions.AsNoTracking()
                           .Where(readSession => readSession.Id == id)
                           .ProjectToType<ReadSessionViewModel>()
                           .FirstOrDefaultAsync()
            ?? throw new Exception("The reading session with such id doesn't exist");
            return readSessionById;
        }

        public async Task<IQueryable<ReadSessionViewModel>> GetAllByUserAsync(Guid userId)
        {
            var readSessionsByUser = _dataContext.ReadSessions.AsNoTracking()
                                     .Where(readSession => readSession.UserId == userId)
                                     .ProjectToType<ReadSessionViewModel>()
            ?? throw new Exception("The reading session with such id doesn't exist");
            return readSessionsByUser;
        }

        public async Task<bool> IsExists(Guid id)
        {
            return await _dataContext.ReadSessions.AnyAsync(p => p.Id == id);
        }

        public async Task<ReadSessionViewModel> UpdateAsync(ReadSessionUpdateModel model, Guid readSessionId)
        {
            var readSessionById = await _dataContext.ReadSessions
            .Where(readSession => readSession.Id == readSessionId)
            .FirstOrDefaultAsync()
            ?? throw new Exception("The reading session with such id doesn't exist");

            var readSession = model.Adapt<ReadSession>();
            _dataContext.Entry(readSession).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return readSession.Adapt<ReadSessionViewModel>();

            //_dataContext.ReadSessions.Update(model.Adapt<ReadSession>());
            //await _dataContext.SaveChangesAsync();
            //return model;
        }

        public async Task<IQueryable<ReadSessionViewModel>> GetAllAsync()
        {
            var readSession = _dataContext.UserBooks.AsNoTracking().ProjectToType<ReadSessionViewModel>();
            return readSession;
        }
    }
}
