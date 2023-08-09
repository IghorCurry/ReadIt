namespace ReadIt.DAL.Entities
{
    public class UserBook
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public double Progress { get; set; }

        public int StopPage { get; set; }
        public bool Finished { get; set; }

    }
}
