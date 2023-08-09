using Microsoft.AspNetCore.Identity;

namespace ReadIt.DAL.Entities
{
    public class User : IdentityUser<Guid>
    {
        public DateTime DateOfBirth { get; set; }
        public ICollection<ReadSession> ReadSessions { get; set; }

        public ICollection<UserBook> UserBook { get; set; } //do we need it here?


    }
}
