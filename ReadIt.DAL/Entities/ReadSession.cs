namespace ReadIt.DAL.Entities
{
    public class ReadSession
    {
        public Guid Id { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public DateTime StartReadingDate { get; set; }
        public int StopPage { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }



        //ReadSession can have only one user, but user can have multiple readsessions
    }
}
