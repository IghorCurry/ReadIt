using ReadIt.DAL.Entities;

namespace ReadIt.BLL.Models.UserModels
{
    public record UserViewModel : UserUpdateModel
    {
        public Guid Id { get; set; }
        public string Email { get; init; }
        public string UserName { get; set; }

        public ICollection<ReadSession> ReadSessions { get; init; }

        public ICollection<UserBook> UserBook { get; init; }

        //public IEnumerable<string> Roles { get; init; } = new List<string>();
    }
}