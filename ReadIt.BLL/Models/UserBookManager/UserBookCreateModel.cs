namespace ReadIt.BLL.Models.UserBookManager
{
    public record UserBookCreateModel
    {
        public Guid UserId { get; init; }

        public Guid BookId { get; init; }
        public double Progress { get; init; }

        public int StopPage { get; init; }
        public bool Finished { get; init; }
    }
}
